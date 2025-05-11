namespace SpotifyTrackView.Resources.Shared;

public class PaginatedResource<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public PaginatedResource(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
    {
        Data = data;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}