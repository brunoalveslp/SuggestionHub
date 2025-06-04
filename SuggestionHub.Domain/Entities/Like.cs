namespace SuggestionHub.Domain.Entities;

public class Like
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int SuggestionId { get; set; }

}