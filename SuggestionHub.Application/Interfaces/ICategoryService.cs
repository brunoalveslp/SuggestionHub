using SuggestionHub.Application.DTOs;

namespace SuggestionHub.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAllAsync();
    Task<CategoryDTO> GetByIdAsync(int id);
    Task CreateAsync(string name);
    Task UpdateAsync(int id, string name);
    Task DeleteAsync(int id);
}
