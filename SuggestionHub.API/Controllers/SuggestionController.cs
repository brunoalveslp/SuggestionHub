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
using SuggestionHub.Domain.Enums;
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
        var suggestionId = await _suggestionService.CreateSuggestionAsync(request.Title,request.Subject, request.Description, request.CategoryId, request.UserId);
        var user = await _userService.GetByIdAsync(request.UserId);

        if(suggestionId == 0)
        {
            return BadRequest("Erro ao criar sugestão, por favor tente novamente!");
        }

        await _suggestionService.AddEventAsync(
            suggestionId, // ID da sugestão será atualizado após a criação
            user.Id,
            user.UserName,
            true,
            "Sugestão Criada",
            "Informamos que devido ao planejamento para os próximos meses já ter sido definido e está atualmente em andamento a análise e o retorno sobre esta sugestão podem levar mais tempo do que o habitual e desejado. Agradecemos pela sua contribuição e compreensão!",
            SuggestionStatus.Pending.ToString() // Novo status pode ser definido se necessário
        );


        return CreatedAtAction(nameof(GetById), new { id = suggestionId }, null);
    }

    [HttpPut("{suggestionId:int}")]
    public async Task<IActionResult> Update(int suggestionId, [FromBody] CreateSuggestionRequest request)
    {
        var user = await _userService.GetByIdAsync(request.UserId);
        if (user is null) return BadRequest("Usuario não encontrado, por favor tente novamente!");

        var result = await _suggestionService.UpdateSuggestionAsync(suggestionId,request.Title, request.Subject, request.Description, request.CategoryId);
        await _suggestionService.AddEventAsync(
            suggestionId,
            user.Id,
            user.UserName,
            true,
            "Sugestão Atualizada",
            "A sugestão foi atualizada com sucesso."
        );

        if (!result)
        {
            return BadRequest("Erro ao atualizar sugestão, por favor tente novamente!");
        }

        return NoContent();
    }

    [HttpPost("{suggestionId:int}/subscription")]
    public async Task<IActionResult> Subscribe(int suggestionId, [FromQuery] string userId)
    {
        await _suggestionService.SubscribeSuggestionAsync(suggestionId, userId);
        return NoContent();
    }
    
    [HttpDelete("{suggestionId:int}/subscription")]
    public async Task<IActionResult> RemoveSubscription(int suggestionId, [FromQuery] string userId)
    {
        await _suggestionService.RemoveSubscriptionAsync(suggestionId, userId);
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

    [HttpPost("{suggestionId}/event")]
    public async Task<IActionResult> AddEvent(int suggestionId, [FromBody] SuggestionEventRequest request)
    {
        await _suggestionService.AddEventAsync(
            suggestionId,
            request.UserId,
            request.UserName,
            request.IsPublic,
            request.Action,
            request.ChangeDescription,
            request.NewStatus
        );

        return NoContent();
    }

}
