using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects.Requests.Profile;

namespace SpotifyTrackView.Validation.Rules;

public class ListenerFavoriteGenreValidator: AbstractValidator<UpdateFavoriteGenresRequest>
{
    public ListenerFavoriteGenreValidator(ApplicationDbContext db)
    {
        RuleFor(x => x.MainGenreIds)
            .MustAsync(async (ids, ct) =>
            {
                var hasInvalid = await db.Genres
                    .Where(g => ids.Contains(g.Id))
                    .AnyAsync(g => g.ParentGenreId != null, ct);
                return !hasInvalid;
            })
            .When(x => x.MainGenreIds.Count > 0)
            .WithMessage("Main genres must not have a parent genre.");
    }
}