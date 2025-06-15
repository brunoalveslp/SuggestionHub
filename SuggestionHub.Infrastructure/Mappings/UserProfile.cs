using AutoMapper;
using SuggestionHub.Application.DTOs;
using SuggestionHub.Infrastructure.Identity.Entities;

namespace SuggestionHub.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile() 
    { 
        CreateMap<UserDTO, ApplicationUser>();
        CreateMap<ApplicationUser, UserDTO>();
    }
}
