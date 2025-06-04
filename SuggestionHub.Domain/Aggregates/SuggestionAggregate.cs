using SuggestionHub.Domain.Entities;
using SuggestionHub.Domain.Enums;

namespace SuggestionHub.Domain.Aggregates;

public class SuggestionAggregate
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public SuggestionStatus status { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    private readonly List<SuggestionEvent> _events = new();
    private readonly List<Like> _likes = new();
    private readonly List<Comment> _comments = new();

    public IReadOnlyCollection<SuggestionEvent> Events => _events.AsReadOnly();
    public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();


    public SuggestionAggregate(string title, string description, int categoryId, string userId)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("É obrigatório um titulo para a sugestão.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("É obrigatório uma descrição para a sugesstão");

        Title = title;
        Description = description;
        CategoryId = categoryId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        status = SuggestionStatus.Pending;

    }

    public void AddLike(string userId)
    {
        if (_likes.Any(x => x.UserId == userId))
            throw new InvalidOperationException("Usuário já curtiu esta sugestão.");

        _likes.Add(new Like
        {
            UserId = userId,
            SuggestionId = Id
        });

    }


    public void RemoveLike(string userId)
    {
        var like = _likes.FirstOrDefault(x => x.UserId == userId);
        if (like == null)
            throw new InvalidOperationException("Usuário não curtiu esta sugestão.");
        _likes.Remove(like);
    }
}