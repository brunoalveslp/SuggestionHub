namespace SuggestionHub.Application.DTOs.Dashboard;

public class TopLikedSuggestionDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int LikeCount { get; set; }
}
