namespace SuggestionHub.API.Requests;

public class CreateSuggestionRequest
{
    public string Title { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; }
}
