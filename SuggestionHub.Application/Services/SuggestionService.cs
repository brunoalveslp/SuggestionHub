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
        var likes = suggestions.Sum(s => s.LikeCount);
        var pending = suggestions.Count(s => s.Status == SuggestionStatus.Pending);

        var topLiked = suggestions
            .OrderByDescending(s => s.LikeCount)
            .Take(10)
            .Select(s => new TopLikedSuggestionDTO
            {
                Id = s.Id,
                Title = s.Title,
                LikeCount = s.LikeCount
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
            TotalLikes = likes,
            PendingCount = pending,
            TopLiked = topLiked,
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

    public async Task CreateSuggestionAsync(string title, string description, int categoryId, string userId)
    {
        var suggestion = new SuggestionAggregate(title, description, categoryId, userId);
        await _repository.AddAsync(suggestion);
        await _repository.SaveChangesAsync();
    }

    public async Task LikeSuggestionAsync(int suggestionId, string userId)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");
        suggestion.AddLike(userId);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

    public async Task RemoveLikeAsync(int suggestionId, string userId)
    {
        var suggestion = await _repository.GetAggregateByIdAsync(suggestionId)
            ?? throw new Exception("Sugestão não encontrada.");
        suggestion.RemoveLike(userId);
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

    public async Task AddEventAsync(int suggestionId, string userId, string userName, string? action = null, string? description = null, string? newStatus = null)
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

        suggestion.AddEvent(userId, userName, action, description, statusToUpdate);
        _repository.Update(suggestion);
        await _repository.SaveChangesAsync();
    }

}
