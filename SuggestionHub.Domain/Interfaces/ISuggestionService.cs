using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Application.Interfaces;

public interface ISuggestionService
{
    Task<SuggestionAggregate?> GetByIdAsync(int id);
    Task<IEnumerable<SuggestionAggregate>> GetAllAsync();
    Task CreateSuggestionAsync(string title, string description, int categoryId, string userId);
    Task LikeSuggestionAsync(int suggestionId, string userId);
    Task RemoveLikeAsync(int suggestionId, string userId);
    Task AddCommentAsync(int suggestionId, string userId, string content);
    Task UpdateCommentAsync(int suggestionId, int commentId, string content);
    Task RemoveCommentAsync(int suggestionId, int commentId);
    Task UpdateStatusAsync(int suggestionId, string newStatus, int userId, string userName);
}
