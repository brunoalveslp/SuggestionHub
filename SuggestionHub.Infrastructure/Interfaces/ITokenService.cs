using SuggestionHub.Application.DTOs;
using SuggestionHub.Infrastructure.Identity.Entities;

namespace SuggestionHub.Infrastructure.Interfaces;

public interface ITokenService
{
    Task<AuthResultDTO> GenerateTokenAsync(ApplicationUser user);
}
