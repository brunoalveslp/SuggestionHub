namespace SuggestionHub.Application.DTOs;

public class OperationResultDTO
{
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public static OperationResultDTO Ok() => new() { Success = true };
    public static OperationResultDTO Fail(IEnumerable<string> errors) => new() { Success = false, Errors = errors.ToList() };
}
