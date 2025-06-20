using SuggestionHub.Domain.Entities;
using SuggestionHub.Domain.Enums;

namespace SuggestionHub.Domain.Aggregates;

public class SuggestionAggregate
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public SuggestionStatus Status { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    private readonly List<SuggestionEvent> _events = new();
    private readonly List<Subscription> _subscriptions = new();
    private readonly List<Comment> _comments = new();

    public IReadOnlyCollection<SuggestionEvent> Events => _events.AsReadOnly();
    public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();


    public SuggestionAggregate(string title, string subject, string description, int categoryId, string userId)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("É obrigatório um titulo para a sugestão.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("É obrigatório uma descrição para a sugesstão");

        Title = title ?? string.Empty;
        Subject = subject ?? string.Empty; 
        Description = description ?? string.Empty;
        CategoryId = categoryId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = SuggestionStatus.Pending;

    }

    public void UpdateTitle(string title)
    {
        if (!string.Equals(Title, title))
        {
            Title = title;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateSubject(string subject)
    {
        if (!string.Equals(Subject, subject))
        {
            Subject = subject;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateDescription(string description)
    {
        if (!string.Equals(Description, description))
        {
            Description = description;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateCategory(int categoryId)
    {
        if (CategoryId != categoryId)
        {
            CategoryId = categoryId;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }

    public void AddSubscription(string userId)
    {
        if (_subscriptions.Any(x => x.UserId == userId))
            throw new InvalidOperationException("Usuário já curtiu esta sugestão.");

        _subscriptions.Add(new Subscription
        {
            UserId = userId,
            SuggestionId = Id
        });

    }
    public void RemoveSubscription(string userId)
    {
        var like = _subscriptions.FirstOrDefault(x => x.UserId == userId);
        if (like == null)
            throw new InvalidOperationException("Usuário não curtiu esta sugestão.");
        _subscriptions.Remove(like);
    }

    public void AddComment(string userId,string userName, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("O conteúdo do comentário não pode ser vazio.");
        _comments.Add(new Comment
        {
            UserId = userId,
            UserName = userName, 
            SuggestionId = Id,
            Content = content,
            CreatedAt = DateTime.UtcNow
        });
    }

    public void UpdateComment(int commentId, string userId, string userName, string content)
    {
        var comment = _comments.FirstOrDefault(x => x.Id == commentId);

        if (comment == null)
            throw new InvalidOperationException("Comentário não encontrado.");
        if (comment.UserId != userId)
            throw new InvalidOperationException("Usuário não tem permissão para editar este comentário.");
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("O nome de usuário não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("O conteúdo do comentário não pode ser vazio.");

        comment.UserName = userName;
        comment.Content = content;
        comment.LastUpdatedAt = DateTime.UtcNow;
    }

    public void RemoveComment(int commentId, string userId, string userRole)
    {
        var comment = _comments.FirstOrDefault(x => x.Id == commentId);

        if (comment == null)
            throw new InvalidOperationException("Comentário não encontrado.");

        if (comment.UserId != userId || userRole != "Admin")
            _comments.Remove(comment);

        else
            throw new InvalidOperationException("Usuário não tem permissão para remover este comentário.");
    }

    public void AddEvent(string userId, string userName, bool isPublic, string? action = null, string? changeDescription = null, SuggestionStatus? newStatus = null)
    {
        bool statusChanged = false;

        if (newStatus.HasValue && Status != newStatus.Value)
        {
            Status = newStatus.Value;
            statusChanged = true;
            isPublic = true; // Se o status foi alterado, o evento é público por padrão
            // Define valores padrão se não foram fornecidos
            action ??= "Status Alterado";
            changeDescription ??= $"Novo status: {newStatus}";
        }

        // Valida que há ao menos algo a ser registrado
        if (!statusChanged && string.IsNullOrWhiteSpace(action) && string.IsNullOrWhiteSpace(changeDescription))
            throw new ArgumentException("Deve ser informada uma ação, descrição ou alteração de status para registrar um evento.");

        _events.Add(new SuggestionEvent
        {
            SuggestionId = Id,
            UserId = userId,
            UserName = userName,
            IsPublic = isPublic,
            ChangeDate = DateTime.UtcNow,
            Action = action,
            ChangeDescription = changeDescription
        });
    }

}