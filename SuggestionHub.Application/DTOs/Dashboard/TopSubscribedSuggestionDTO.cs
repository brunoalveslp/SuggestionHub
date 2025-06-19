namespace SuggestionHub.Application.DTOs.Dashboard;

public class TopSubscribedSuggestionDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int SubscriptionCount { get; set; }
}
