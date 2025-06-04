using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int SuggestionId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public SuggestionAggregate Suggestion { get; set; }
}