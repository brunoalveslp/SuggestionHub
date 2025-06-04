using Microsoft.AspNetCore.Mvc;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Infrastructure.Interfaces;

namespace SuggestionHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var result = await _userService.RegisterAsync(dto.Email, dto.Password, dto.DisplayName, dto.Role, dto.IdRevenda);

        if (!result.Success)
        {
            string errors = string.Join(".\n*", result.Errors);
            return BadRequest("Erro ao registrar usuário. Por favor, verifique os dados e tente novamente. \n"+ errors);
        }

        return Ok("Usuário registrado com sucesso.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var result = await _userService.LoginAsync(dto.Email, dto.Password);
        if (result == null)
            return Unauthorized("Credenciais inválidas. Por favor verifique o usuario e senha e tente novamente.");

        return Ok(result);
    }
}
