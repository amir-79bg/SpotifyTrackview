using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Profile;

namespace SpotifyTrackView.Validation.Rules;

public class UpdateProfileValidator: AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileValidator(ApplicationDbContext db)
    {
        RuleFor(x => x.Country)
            .MustAsync(async (country, _) => await db.Countries.AnyAsync(c => c.Iso2 == country))
            .When(x => x.Country != null)
            .WithMessage("Invalid country.");
        
        RuleFor(x => x.Region)
            .MustAsync(async (region, _) => await db.Regions.AnyAsync(r => r.Iso2 == region))
            .When(x => x.Region != null)
            .WithMessage("Invalid region.");
        
        RuleFor(x => x.ThumbnailUrl)
            .Cascade(CascadeMode.Stop)
            .Must(file => file == null || file.Length > 0)
            .WithMessage("File cannot be empty.")
            .Must(file => file == null || new[] { "image/jpeg", "image/png", "image/webp" }.Contains(file.ContentType))
            .When(x => x.ThumbnailUrl != null)
            .WithMessage("Only JPG, PNG, and WEBP images are allowed.");
    }
}