using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuggestionHub.API.Requests;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Application.DTOs.Dashboard;
using SuggestionHub.Application.DTOs.Filter;
using SuggestionHub.Application.DTOs.Result;
using SuggestionHub.Application.Interfaces;
using SuggestionHub.Infrastructure.Interfaces;

namespace SuggestionHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class SuggestionController : ControllerBase
{
    private readonly ISuggestionService _suggestionService;
    private readonly IUserService _userService;
    public SuggestionController(ISuggestionService suggestionService, IUserService userService)
    {
        _suggestionService = suggestionService;
        _userService = userService;
    }

    [HttpGet("monthly-summary")]
    [Authorize]
    public async Task<ActionResult<DashboardSummaryDTO>> GetMonthlySummary([FromQuery] int month, [FromQuery] int year)
    {

        var result = await _suggestionService.GetDashboardSummaryAsync(month, year);

        if (result is null)
        {
            return NotFound("Nenhuma sugestão encontrada!");
        }

        return Ok(result);
    }


    [HttpGet]
    public async Task<ActionResult<PaginatedResult<SuggestionSummaryDTO>>> GetAll([FromQuery] SuggestionFilterRequest filter,
        [FromQuery] string currentUserId)
    {
        var result = await _suggestionService.GetAllAsync(filter, currentUserId);
        return Ok(result);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<SuggestionDTO>> GetById(int id)
    {
        var suggestion = await _suggestionService.GetByIdAsync(id);
        return Ok(suggestion);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSuggestionRequest request)
    {
        await _suggestionService.CreateSuggestionAsync(request.Title, request.Description, request.CategoryId, request.UserId);
        return CreatedAtAction(nameof(GetById), new { id = 0 }, null); // Atualize com ID se possível
    }

    [Authorize]
    [HttpPost("{suggestionId:int}/like")]
    public async Task<IActionResult> Like(int suggestionId, [FromQuery] string userId)
    {
        await _suggestionService.LikeSuggestionAsync(suggestionId, userId);
        return NoContent();
    }
    [Authorize]
    [HttpDelete("{suggestionId:int}/like")]
    public async Task<IActionResult> RemoveLike(int suggestionId, [FromQuery] string userId)
    {
        Console.WriteLine($"Removing like for suggestion {suggestionId} by user {userId}");
        await _suggestionService.RemoveLikeAsync(suggestionId, userId);
        return NoContent();
    }

    [HttpPost("{suggestionId:int}/comments")]
    public async Task<IActionResult> AddComment(int suggestionId, [FromBody] CommentRequest request)
    {
        var user = await _userService.GetByIdAsync(request.UserId);
        if (user is null) return BadRequest("Usuario não encontrado, por favor tente novamente!");

        await _suggestionService.AddCommentAsync(suggestionId, user.Id, user.UserName, request.Content);
        return NoContent();
    }

    [HttpPut("{suggestionId:int}/comments/{commentId:int}")]
    public async Task<IActionResult> UpdateComment(int suggestionId, int commentId, [FromBody] CommentUpdateRequest request)
    {
        var user = await _userService.GetByIdAsync(request.UserId);
        if (user is null) return BadRequest("Usuario não encontrado, por favor tente novamente!");

        await _suggestionService.UpdateCommentAsync(suggestionId, commentId, user.Id, user.UserName, request.Content);
        return NoContent();
    }

    [HttpDelete("{suggestionId:int}/comments/{commentId:int}")]
    public async Task<IActionResult> RemoveComment(int suggestionId, int commentId, [FromHeader] string userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user is null) return BadRequest("Usuario não encontrado, por favor tente novamente!");

        await _suggestionService.RemoveCommentAsync(suggestionId, commentId, user.Id, user.Role);
        return NoContent();
    }

    [HttpPatch("{suggestionId:int}/status")]
    public async Task<IActionResult> UpdateStatus(int suggestionId, [FromBody] StatusUpdateRequest request)
    {
        await _suggestionService.UpdateStatusAsync(suggestionId, request.NewStatus, request.UserId, request.UserName);
        return NoContent();
    }
}
