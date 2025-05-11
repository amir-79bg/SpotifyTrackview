using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Resources.Shared;

namespace SpotifyTrackView.Helpers;

public static class PaginationHelper
{
    public static async Task<PaginatedResource<TDto>> PaginateAsync<TEntity, TDto>(
        IQueryable<TEntity> query,
        Func<TEntity, TDto> transform,
        int page,
        int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var data = items.Select(transform);

        return new PaginatedResource<TDto>(data, totalCount, page, pageSize);
    }
}