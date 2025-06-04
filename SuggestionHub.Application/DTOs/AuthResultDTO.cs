namespace SuggestionHub.Application.DTOs;

public class AuthResultDTO
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public int IdRevenda { get; set; }
    public List<string> Roles { get; set; }
}
