namespace SuggestionHub.API.Requests;

public class SuggestionEventRequest
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string? Action { get; set; }
    public string? ChangeDescription { get; set; }
    public string? NewStatus { get; set; } // opcional, ex: "Approved"
}
