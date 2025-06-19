using Microsoft.EntityFrameworkCore;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Application.DTOs.Filter;
using SuggestionHub.Application.DTOs.Result;
using SuggestionHub.Application.Interfaces;
using SuggestionHub.Domain.Aggregates;
using SuggestionHub.Domain.Enums;
using SuggestionHub.Infrastructure.Context;

namespace SuggestionHub.Infrastructure.Repositories;

public class SuggestionRepository : ISuggestionRepository
{
    private readonly AppDbContext _context;

    public SuggestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SuggestionSummaryDTO>> GetSuggestionsByMonthAsync(int month, int year)
    {
        var result = await _context.Suggestions
            .AsNoTracking()
            .Include(s => s.Comments)
            .Include(s => s.Subscriptions)
            .Include(s => s.Events)
            .Where(s => s.CreatedAt.Month == month && s.CreatedAt.Year == year)
            .ToListAsync();

        var summaries = result.Select(s => new SuggestionSummaryDTO
        {
            Id = s.Id,
            Title = s.Title,
            Subject = s.Subject,
            Description = s.Description,
            Status = s.Status,
            CategoryId = s.CategoryId,
            CreatedAt = s.CreatedAt,
            CommentCount = s.Comments?.Count ?? 0,
            SubscriptionCount = s.Subscriptions?.Count ?? 0,
            HasUserSubscribed = false // este valor é específico para o usuário logado e pode ser ajustado depois
        }).ToList();

        return summaries;
    }

    public async Task<IEnumerable<SuggestionAggregate>> GetAllAsync()
        => await _context.Suggestions
                         .Include(s => s.Comments)
                         .Include(s => s.Subscriptions)
                         .Include(s => s.Events)
                         .ToListAsync();

    public async Task<PaginatedResult<SuggestionSummaryDTO>> GetFilteredAsync(SuggestionFilterRequest filter, string currentUserId)
    {
        var query = _context.Suggestions
            .Include(c => c.Comments)
            .Include(l => l.Subscriptions)
            .Include(e => e.Events)
            .AsQueryable();

        if (filter.CategoryId.HasValue)
            query = query.Where(s => s.CategoryId == filter.CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            query = query.Where(s =>
                s.Title.ToLower().Contains(term) ||
                s.Description.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status) &&
            Enum.TryParse<SuggestionStatus>(filter.Status, out var statusEnum))
        {
            query = query.Where(s => s.Status == statusEnum);
        }

        if (!string.IsNullOrWhiteSpace(filter.CreatedByUserId))
            query = query.Where(s => s.UserId == filter.CreatedByUserId);

        var totalItems = await query.CountAsync();

        // Projeção para DTO com campos adicionais (para sort)
        var projected = query.Select(s => new
        {
            s.Id,
            s.Title,
            s.Subject,
            s.Description,
            s.Status,
            s.CategoryId,
            s.CreatedAt,
            CommentCount = s.Comments.Count(),
            SubscriptionCount = s.Subscriptions.Count(l => l.UserId != null && l.UserId != ""),
            HasUserSubscribed = s.Subscriptions.Any(l => l.UserId == currentUserId)
        });

        // Aplicar ordenação dinâmica
        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            projected = filter.SortBy switch
            {
                "Title" => filter.Descending
                    ? projected.OrderByDescending(s => s.Title)
                    : projected.OrderBy(s => s.Title),

                "SubscriptionCount" => filter.Descending
                    ? projected.OrderByDescending(s => s.SubscriptionCount)
                    : projected.OrderBy(s => s.SubscriptionCount),

                "CreatedAt" or _ => filter.Descending
                    ? projected.OrderByDescending(s => s.CreatedAt)
                    : projected.OrderBy(s => s.CreatedAt),
            };
        }

        var items = await projected
            .Skip(filter.Offset).Take(filter.PageSize)
            .Select(s => new SuggestionSummaryDTO
            {
                Id = s.Id,
                Title = s.Title,
                Subject = s.Subject,
                Description = s.Description.Length > 200
                    ? s.Description.Substring(0, 200) + "..."
                    : s.Description,
                Status = s.Status,
                CategoryId = s.CategoryId,
                CreatedAt = s.CreatedAt,
                CommentCount = s.CommentCount,
                SubscriptionCount = s.SubscriptionCount,
                HasUserSubscribed = s.HasUserSubscribed
            })
            .ToListAsync();

        return new PaginatedResult<SuggestionSummaryDTO>
        {
            Items = items,
            HasMore = filter.Offset + filter.PageSize < totalItems
        };
    }


    public async Task<SuggestionDTO?> GetByIdAsync(int id)
    {
        return await _context.Suggestions
            .Include(c => c.Comments)
            .Include(l => l.Subscriptions)
            .Include(e => e.Events)
            .Where(q => q.Id == id)
            .Select(q => new SuggestionDTO
            {
                Id = q.Id,
                Title = q.Title,
                Subject = q.Subject,
                Description = q.Description,
                Status = q.Status,
                CategoryId = q.CategoryId,
                UserId = q.UserId,
                CreatedAt = q.CreatedAt,
                LastUpdatedAt = q.LastUpdatedAt,
                Events = q.Events.Select(e => new SuggestionEventDTO
                {
                    Id = e.Id,
                    SuggestionId = e.SuggestionId,
                    UserId = e.UserId,
                    UserName = e.UserName,
                    Action = e.Action,
                    ChangeDescription = e.ChangeDescription,
                    ChangeDate = e.ChangeDate
                }).ToList(),
                SubscriptionCount = q.Subscriptions.Count,
                HasUserSubscribed = q.Subscriptions.Any(l => l.UserId == q.UserId),
                Comments = q.Comments.Select(c => new CommentDTO
                {
                    Id = c.Id,
                    SuggestionId = c.SuggestionId,
                    UserId = c.UserId,
                    UserName = c.UserName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<SuggestionAggregate?> GetAggregateByIdAsync(int id)
    {
        return await _context.Suggestions
            .Include(s => s.Subscriptions)
            .Include(s => s.Comments)
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);
    }



    public async Task AddAsync(SuggestionAggregate suggestion)
        => await _context.Suggestions.AddAsync(suggestion);

    public void Update(SuggestionAggregate suggestion)
        => _context.Suggestions.Update(suggestion);

    public void Delete(SuggestionAggregate suggestion)
        => _context.Suggestions.Remove(suggestion);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
