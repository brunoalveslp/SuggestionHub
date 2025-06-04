using SuggestionHub.Application.DTOs;
using SuggestionHub.Infrastructure.Identity.Entities;

namespace SuggestionHub.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser> GetByEmailAsync(string email);
    Task<ApplicationUser> GetByIdAsync(string id);
    Task<OperationResultDTO> CreateAsync(ApplicationUser user, string password);
    Task AddRoleAsync(ApplicationUser user, string role);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
