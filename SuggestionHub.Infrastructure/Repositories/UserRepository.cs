using Microsoft.AspNetCore.Identity;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Infrastructure.Identity.Entities;
using SuggestionHub.Infrastructure.Interfaces;

namespace SuggestionHub.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task AddRoleAsync(ApplicationUser user, string role) => 
        await _userManager.AddToRoleAsync(user, role);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password) => 
        await _userManager.CheckPasswordAsync(user, password);

    public async Task<OperationResultDTO> CreateAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded) return OperationResultDTO.Ok();

        var errors = result.Errors.Select(e => e.Description);

        return OperationResultDTO.Fail(errors);
    }

    public async Task<ApplicationUser> GetByEmailAsync(string email) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser> GetByIdAsync(string id) =>
        await _userManager.FindByIdAsync(id);
}
