namespace SuggestionHub.Application.DTOs;

public class CommentDTO
{
    public int Id { get; set; }
    public int SuggestionId { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
}
