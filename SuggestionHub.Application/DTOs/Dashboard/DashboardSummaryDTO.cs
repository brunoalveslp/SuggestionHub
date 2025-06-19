namespace SuggestionHub.Application.DTOs.Dashboard;

public class DashboardSummaryDTO
{
    public int TotalSuggestions { get; set; }
    public int TotalSubscriptions { get; set; }
    public int PendingCount { get; set; }
    public List<DayCountDTO> SuggestionsByDay { get; set; } = [];
    public List<TopSubscribedSuggestionDTO> TopSubscribed { get; set; } = [];
}
