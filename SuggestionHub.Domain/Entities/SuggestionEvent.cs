using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Domain.Entities;

public class SuggestionEvent
{
    public int Id { get; set; }
    public int SuggestionId { get; set; }
    public SuggestionAggregate Suggestion { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public bool IsPublic { get; set; } = false;
    public DateTime ChangeDate { get; set; }
    public string Action { get; set; } 
    public string? ChangeDescription { get; set; } 
}
