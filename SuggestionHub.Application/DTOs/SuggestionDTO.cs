using SuggestionHub.Domain.Enums;

namespace SuggestionHub.Application.DTOs;

public class SuggestionDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public SuggestionStatus Status { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }

    public List<SuggestionEventDTO> Events { get; set; } = new();
    public int LikeCount { get; set; }
    public bool HasUserLiked { get; set; }
    public List<CommentDTO> Comments { get; set; } = new();
}
