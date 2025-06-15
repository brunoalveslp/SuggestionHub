using SuggestionHub.Application.DTOs;
using SuggestionHub.Application.DTOs.Filter;
using SuggestionHub.Application.DTOs.Result;
using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Application.Interfaces;

public interface ISuggestionRepository
{
    Task<List<SuggestionSummaryDTO>> GetSuggestionsByMonthAsync(int month, int year);
    Task<SuggestionDTO?> GetByIdAsync(int id);
    Task<SuggestionAggregate?> GetAggregateByIdAsync(int id);
    Task<PaginatedResult<SuggestionSummaryDTO>> GetFilteredAsync(SuggestionFilterRequest filter, string currentUserId);
    Task<IEnumerable<SuggestionAggregate>> GetAllAsync();
    Task AddAsync(SuggestionAggregate suggestion);
    void Update(SuggestionAggregate suggestion);
    void Delete(SuggestionAggregate suggestion);
    

    Task SaveChangesAsync();
}
