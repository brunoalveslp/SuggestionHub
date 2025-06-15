namespace SuggestionHub.Application.DTOs;

public class LikeDTO
{
    public int Id { get; set; }
    public int SuggestionId { get; set; }
    public string UserId { get; set; } = null!;
}
