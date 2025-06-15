using Microsoft.AspNetCore.Identity;
using SuggestionHub.Application.DTOs;

namespace SuggestionHub.Infrastructure.Interfaces;

public interface IUserService
{
    Task<UserDTO> GetByIdAsync(string id);
    Task<OperationResultDTO> RegisterAsync(string email, string password, string displayName, string role, int idRevenda);
    Task<AuthResultDTO> LoginAsync(string email, string password);
}
