using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Domain.Interfaces;

public interface ISuggestionRepository
{
    Task<SuggestionAggregate?> GetByIdAsync(int id);
    Task<IEnumerable<SuggestionAggregate>> GetAllAsync();
    Task AddAsync(SuggestionAggregate suggestion);
    void Update(SuggestionAggregate suggestion);
    void Delete(SuggestionAggregate suggestion);
    Task SaveChangesAsync();
}
