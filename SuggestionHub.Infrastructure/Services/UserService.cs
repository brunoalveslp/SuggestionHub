using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Infrastructure.Identity.Entities;
using SuggestionHub.Infrastructure.Interfaces;

namespace SuggestionHub.Infrastructure.Services;

public class UserService : IUserService

{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    public async Task<UserDTO> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return null;

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<AuthResultDTO> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, password))
            return null;

        return await _tokenService.GenerateTokenAsync(user);

    }

    public async Task<OperationResultDTO> RegisterAsync(string email, string password, string displayName, string role, int idRevenda)
    {
        var user = new ApplicationUser { Email = email, UserName = email, IdRevenda = idRevenda, DisplayName = displayName };
        var result = await _userRepository.CreateAsync(user, password);

        if (!result.Success)
        {
            return result;
        }

        await _userRepository.AddRoleAsync(user, role);
        return OperationResultDTO.Ok();

    }

}
