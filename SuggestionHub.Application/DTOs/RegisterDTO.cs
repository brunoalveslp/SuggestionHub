﻿namespace SuggestionHub.Application.DTOs;

public class RegisterDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string DisplayName { get; set; }
    public string Role { get; set; }
    public int IdRevenda { get; set; }
}
