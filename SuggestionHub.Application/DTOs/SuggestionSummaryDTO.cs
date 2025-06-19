using SuggestionHub.Domain.Enums;

public class SuggestionSummaryDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Subject { get; set; }
    public string Description { get; set; } 
    public SuggestionStatus Status { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CommentCount { get; set; }
    public int SubscriptionCount { get; set; }
    public bool HasUserSubscribed { get; set; }
}
