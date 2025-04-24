using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests;
using SpotifyTrackView.DataTransferObjects.Requests.Profile;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Options;
using SpotifyTrackView.Resources;
using SpotifyTrackView.Validation.Rules;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

[ApiController]
[Route("/api/v1/profile")]
public class ProfileController : BaseApiController
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly FileUploadOptions _opts;


    public ProfileController(
        ApplicationDbContext context,
        IWebHostEnvironment env,
        IOptions<FileUploadOptions> opts
    )
    {
        _context = context;
        _env = env;
        _opts = opts.Value;
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "InfluencerScheme,ListenerScheme,ArtistScheme")]
    public async Task<IActionResult> UpdateProfile(
        [FromForm] UpdateProfileRequest request,
        IValidator<UpdateProfileRequest> validator
    )
    {
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return ValidationProblem(ModelState);
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        string? url = null;
        if (!(request.ThumbnailUrl == null || request.ThumbnailUrl.Length == 0))
        {
            var uploadDir = Path.Combine(_env.WebRootPath, _opts.BasePath);
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            var ext = Path.GetExtension(request.ThumbnailUrl.FileName);
            var filename = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadDir, filename);

            using var stream = new FileStream(filePath, FileMode.Create);
            await request.ThumbnailUrl.CopyToAsync(stream);

            url = $"{_opts.RequestPath}/{filename}";
        }


        AppUser? user = role switch
        {
            nameof(Entity.Artist) => await _context.Artists.FindAsync(userId),
            nameof(Entity.Influencer) => await _context.Influencers.FindAsync(userId),
            nameof(Entity.Listener) => await _context.Listeners.FindAsync(userId),
            _ => throw new Exception()
        };

        AppUser userEntity = request.TransformToAppUser(userId, url);

        await _context.SaveChangesAsync();

        return Ok(UserResource.From(userEntity, Request));
    }
}