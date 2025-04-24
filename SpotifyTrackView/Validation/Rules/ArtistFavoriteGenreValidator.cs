using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Profile;

namespace SpotifyTrackView.Validation.Rules;

public class ArtistFavoriteGenreValidator : AbstractValidator<UpdateFavoriteGenresRequest>
{
    public ArtistFavoriteGenreValidator(ApplicationDbContext db)
    {
        RuleFor(x => x.MainGenreIds)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("You must select at least one main genre.")
            .Must(ids => ids.Count >= 1 && ids.Count <= 3).WithMessage("You must select between 1 and 3 main genres.")
            .MustAsync(async (ids, ct) =>
            {
                var hasInvalid = await db.Genres
                    .Where(g => ids.Contains(g.Id))
                    .AnyAsync(g => g.ParentGenreId != null, ct);
                return !hasInvalid;
            }).WithMessage("Main genres must not have a parent genre.");


        RuleFor(x => x.SubGenreIds)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("You must select at least one sub genre.")
            .Must(ids => ids.Count >= 1 && ids.Count <= 15).WithMessage("You must select between 1 and 15 sub genres.")
            .MustAsync(async (ids, ct) =>
            {
                return await db.Genres
                    .Where(g => ids.Contains(g.Id))
                    .AllAsync(g => g.ParentGenreId != null, ct);
            }).WithMessage("All sub genres must have a parent genre.");

        RuleFor(x => x.OtherGenreIds)
            .Cascade(CascadeMode.Stop)
            .Must(ids => ids.Count >= 0 && ids.Count <= 3).WithMessage("You must select between 1 and 3 other genres.")
            .MustAsync(async (ids, ct) =>
            {
                var hasInvalid = await db.Genres
                    .Where(g => ids.Contains(g.Id))
                    .AnyAsync(g => g.ParentGenreId != null, ct);
                return !hasInvalid;
            }).When(x => x.OtherGenreIds.Count == 0).WithMessage("Other genres must not have a parent genre.");
    }
}
// Main Genre 1 till 3
// 1 till 15
// other genres 1 till 3