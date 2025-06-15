using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SuggestionHub.Infrastructure.Identity.Entities;
using SuggestionHub.Infrastructure.Identity;
using System.Security.Claims;
using System.Text;
using SuggestionHub.Infrastructure.Interfaces;
using SuggestionHub.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SuggestionHub.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
    }

    public async Task<AuthResultDTO> GenerateTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("displayName", user.DisplayName ?? string.Empty)
    };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("role", role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new AuthResultDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expires,
            UserId = user.Id,
            Email = user.Email,
            IdRevenda = user.IdRevenda,
            DisplayName = user.DisplayName,
            Roles = roles.ToList()
        };
    }
}
