﻿using SuggestionHub.Domain.Aggregates;

namespace SuggestionHub.Domain.Entities;

public class Subscription
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int SuggestionId { get; set; }
    public SuggestionAggregate Suggestion { get; set; }

}