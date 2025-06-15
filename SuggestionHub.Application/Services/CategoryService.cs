using AutoMapper;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Application.Interfaces;
using SuggestionHub.Domain.Entities;


namespace SuggestionHub.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }

    public async Task<CategoryDTO> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Categoria não encontrada");
        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task CreateAsync(string name)
    {
        var category = new Category { Name = name };
        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, string name)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Categoria não encontrada");

        category.Name = name;
        _repository.Update(category);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Categoria não encontrada");

        _repository.Delete(category);
        await _repository.SaveChangesAsync();
    }
}
