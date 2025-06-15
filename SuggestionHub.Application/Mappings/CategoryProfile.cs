using AutoMapper;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Domain.Entities;

namespace SuggestionHub.Application.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile() 
    {
        // Category ↔ CategoryDto
        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryDTO, Category>();
    }
}
