using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Options;
using SpotifyTrackView.Resources;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

[ApiController]
[Route("/api/v1/profile")]
public class ProfileController: BaseApiController
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly FileUploadOptions _opts;


    public ProfileController(ApplicationDbContext context, IWebHostEnvironment env, IOptions<FileUploadOptions> opts)
    {
        _context = context;
        _env = env;
        _opts = opts.Value;
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "InfluencerScheme,ListenerScheme,ArtistScheme")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto dto)
    {
        if (! await _context.Countries.AnyAsync(c => c.Iso2 == dto.Country))
        {
            return ValidationError("Country", "Invalid country.");
        }
        
        if (! await _context.Regions.AnyAsync(r => r.Iso2 == dto.Region))
        {
            return ValidationError("Region", "Invalid region.");
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        string? url = null;
        if (! (dto.ThumbnailUrl == null || dto.ThumbnailUrl.Length == 0))
        {
            if (Request.Form.Files.Count > 1)
            {
                return ValidationError("ThumbnailUrl", "Only one file can be uploaded.");
            }
            // Validate content type
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(dto.ThumbnailUrl.ContentType))
            {
                return ValidationError("ThumbnailUrl", "Only JPG, PNG, and WEBP images are allowed.");
            }
                
            var uploadDir = Path.Combine(_env.WebRootPath, _opts.BasePath);
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            // 2) Build a unique filename
            var ext = Path.GetExtension(dto.ThumbnailUrl.FileName);
            var filename = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadDir, filename);

            // 3) Save to disk
            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.ThumbnailUrl.CopyToAsync(stream);

            // 4) Return the URL, matching how you serve static files
            url = $"{_opts.RequestPath}/{filename}";
        }
        

        AppUser user = role switch
        {
            nameof(Entity.Artist) => await _context.Artists.FindAsync(userId),
            nameof(Entity.Influencer) => await _context.Influencers.FindAsync(userId),
            nameof(Entity.Listener) => await _context.Listeners.FindAsync(userId),
        };

        AppUser userEntity = dto.TransformToAppUser(url);
        
        await _context.SaveChangesAsync();

        return Ok(UserResource.From(userEntity, Request));
    }
}

/*
FirstName:Iman
LastName:Parvizi
Country:IR
Region:IR
ThumbnailUrl:https://kir.com
*/