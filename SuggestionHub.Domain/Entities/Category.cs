using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<SuggestionAggregate> Suggestions { get; set; }
}