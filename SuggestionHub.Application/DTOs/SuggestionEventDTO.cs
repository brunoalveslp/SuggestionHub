namespace SuggestionHub.Application.DTOs;

public class SuggestionEventDTO
{
    public int Id { get; set; }
    public int SuggestionId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string? ChangeDescription { get; set; }
    public DateTime ChangeDate { get; set; }
}
