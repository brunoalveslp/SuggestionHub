using SuggestionHub.Application.DTOs;
using SuggestionHub.Application.DTOs.Dashboard;
using SuggestionHub.Application.DTOs.Filter;
using SuggestionHub.Application.DTOs.Result;
using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Application.Interfaces;

public interface ISuggestionService
{
    Task<DashboardSummaryDTO> GetDashboardSummaryAsync(int month, int year);
    Task<SuggestionDTO> GetByIdAsync(int id);
    Task<PaginatedResult<SuggestionSummaryDTO>> GetAllAsync(SuggestionFilterRequest filter, string currentUserId);
    Task<int> CreateSuggestionAsync(string title,string subject,string description, int categoryId, string userId);
    Task<bool> UpdateSuggestionAsync(int suggestionId,string title, string subject, string description, int categoryId);
    Task SubscribeSuggestionAsync(int suggestionId, string userId);
    Task RemoveSubscriptionAsync(int suggestionId, string userId);
    Task AddCommentAsync(int suggestionId, string userId, string userName, string content);
    Task UpdateCommentAsync(int suggestionId, int commentId, string userId, string userName, string content);
    Task RemoveCommentAsync(int suggestionId, int commentId, string userId, string userRole);
    Task AddEventAsync(int suggestionId, string userId, string userName, bool isPublic, string? action = null, string? description = null, string? newStatus = null);

}
