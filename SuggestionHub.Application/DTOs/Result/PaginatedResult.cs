namespace SuggestionHub.Application.DTOs.Result;
public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public bool HasMore { get; set; }
}
