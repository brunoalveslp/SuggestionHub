namespace SuggestionHub.API.Requests;

public class StatusUpdateRequest
{
    public string NewStatus { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
}
