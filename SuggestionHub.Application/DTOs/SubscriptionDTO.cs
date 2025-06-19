namespace SuggestionHub.Application.DTOs;

public class SubscriptionDTO
{
    public int Id { get; set; }
    public int SuggestionId { get; set; }
    public string UserId { get; set; } = null!;
}
