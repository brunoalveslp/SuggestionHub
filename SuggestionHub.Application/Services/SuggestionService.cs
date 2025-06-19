using AutoMapper;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Application.DTOs.Dashboard;
using SuggestionHub.Application.DTOs.Filter;
using SuggestionHub.Application.DTOs.Result;
using SuggestionHub.Application.Interfaces;
using SuggestionHub.Domain.Aggregates;
using SuggestionHub.Domain.Enums;

namespace SuggestionHub.Application.Services;

public class SuggestionService : ISuggestionService
{
    private readonly ISuggestionRepository _repository;
    private readonly IMapper _mapper;

    public SuggestionService(ISuggestionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;   
    }

    public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync(int month, int year)
    {
        var suggestions = await _repository.GetSuggestionsByMonthAsync(month, year);

        var total = suggestions.Count;
        var subscriptions = suggestions.Sum(s => s.SubscriptionCount);
        var pending = suggestions.Count(s => s.Status == SuggestionStatus.Pending);

        var topSubscribed = suggestions
            .OrderByDescending(s => s.SubscriptionCount)
            .Take(10)
            .Select(s => new TopSubscribedSuggestionDTO
            {
                Id = s.Id,
                Title = s.Title,
                SubscriptionCount = s.SubscriptionCount
            })
            .ToList();

        var groupedByDay = suggestions
            .GroupBy(s => s.CreatedAt.Date)
            .Select(g => new DayCountDTO
            {
                Day = g.Key.ToString("dd/MM"),
                Count = g.Count()
            })
            .ToList();

        return new DashboardSummaryDTO
        {
            TotalSuggestions = total,
            TotalSubscriptions = subscriptions,
            PendingCount = pending,
            TopSubscribed = topSubscribed,
            SuggestionsByDay = groupedByDay
        };
    }

    public async Task<PaginatedResult<SuggestionSummaryDTO>> GetAllAsync(SuggestionFilterRequest filter, string currentUserId)
    {
        var paginatedSuggestions = await _repository.GetFilteredAsync(filter, currentUserId);

        return new PaginatedResult<SuggestionSummaryDTO>
        {
            Items = _mapper.Map<IEnumerable<SuggestionSummaryDTO>>(paginatedSuggestions.Items),
            HasMore = paginatedSuggestions.HasMore
        };
    }

    public async Task<SuggestionDTO> GetByIdAsync(int id)
    {
        var suggestion = await _repository.GetByIdAsync(id);
        return _mapper.Map<SuggestionDTO>(suggestion)
               ?? throw new Exception("Sugestão não encontrada.");
    }

    public async Task<int> CreateSuggestionAsync(string title, string subject, string description, int categoryId, string userId)
    {
        var suggestion = new SuggestionAggregate(title, subject, description, categoryId, userId);
        await _repository.AddAsync(suggestion);
        await _repository.SaveChangesAsync();

        return suggestion.Id;
    }

    public async Task<bool> UpdateSuggestionAsync(int suggestionId, string title, string subject, string description, int categoryId)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");

        bool hasChanges = false;

        if (!string.Equals(suggestion.Title, title, StringComparison.Ordinal) || suggestion.Title is null)
        {
            suggestion.UpdateTitle(title);
            hasChanges = true;
        }

        if (!string.Equals(suggestion.Subject, subject, StringComparison.Ordinal) || suggestion.Subject is null)
        {
            suggestion.UpdateSubject(subject);
            hasChanges = true;
        }

        if (!string.Equals(suggestion.Description, description, StringComparison.Ordinal) || suggestion.Description is null)
        {
            suggestion.UpdateDescription(description);
            hasChanges = true;
        }

        if (suggestion.CategoryId != categoryId)
        {
            suggestion.UpdateCategory(categoryId);
            hasChanges = true;
        }

        if (!hasChanges)
            return hasChanges;

        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
        return hasChanges;
    }

    public async Task SubscribeSuggestionAsync(int suggestionId, string userId)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");
        suggestion.AddSubscription(userId);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

    public async Task RemoveSubscriptionAsync(int suggestionId, string userId)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");
        suggestion.RemoveSubscription(userId);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

    public async Task AddCommentAsync(int suggestionId, string userId, string userName, string content)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");
        suggestion.AddComment(userId, userName, content);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(int suggestionId, int commentId, string userId, string userName, string content)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");
        suggestion.UpdateComment(commentId, userId, userName, content);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

    public async Task RemoveCommentAsync(int suggestionId, int commentId, string userId, string userRole)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId);

        if (suggestion is null)
            throw new ArgumentNullException("Sugestão não encontrada!");

        suggestion.RemoveComment(commentId, userId, userRole);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

    public async Task AddEventAsync(int suggestionId, string userId, string userName, bool isPublic, string? action = null, string? description = null, string? newStatus = null)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");

        SuggestionStatus? statusToUpdate = null;

        if (!string.IsNullOrWhiteSpace(newStatus))
        {
            if (!Enum.TryParse<SuggestionStatus>(newStatus, out var parsedStatus))
                throw new ArgumentException("Status inválido.");

            statusToUpdate = parsedStatus;
        }

        suggestion.AddEvent(userId, userName, isPublic, action, description, statusToUpdate);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

}
