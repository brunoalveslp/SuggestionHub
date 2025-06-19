using AutoMapper;
using SuggestionHub.Domain.Aggregates;
using SuggestionHub.Domain.Entities;
using SuggestionHub.Application.DTOs;


namespace SuggestionHub.Application.Mappings;

public class SuggestionProfile : Profile
{
    public SuggestionProfile()
    {
        // SuggestionAggregate ↔ SuggestionDto
        CreateMap<SuggestionAggregate, SuggestionDTO>();
        CreateMap<SuggestionDTO, SuggestionAggregate>();

        // SuggestionAggregate ↔ SuggestionSummaryDto
        CreateMap<SuggestionAggregate, SuggestionSummaryDTO>();

        // SuggestionEvent ↔ SuggestionEventDto
        CreateMap<SuggestionEvent, SuggestionEventDTO>();
        CreateMap<SuggestionEventDTO, SuggestionEvent>();

        // Like ↔ LikeDto
        CreateMap<Subscription, SubscriptionDTO>();
        CreateMap<SubscriptionDTO, Subscription>();

        // Comment ↔ CommentDto
        CreateMap<Comment, CommentDTO>();
        CreateMap<CommentDTO, Comment>();
    }
}
