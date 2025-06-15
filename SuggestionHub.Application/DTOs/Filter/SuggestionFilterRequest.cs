namespace SuggestionHub.Application.DTOs.Filter;

public class SuggestionFilterRequest
{
    public int? CategoryId { get; set; }
    public string? Status { get; set; }
    public string? CreatedByUserId { get; set; }

    public string? SearchTerm { get; set; }

    public int PageSize { get; set; } = 10;
    public int Offset { get; set; } = 0;

    public string? SortBy { get; set; } = "CreatedAt";
    public bool Descending { get; set; } = true;
}
