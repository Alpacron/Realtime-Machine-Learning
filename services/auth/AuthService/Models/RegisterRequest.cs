using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(4)]
    public string Username { get; set; } = default!;

    [Required, MinLength(4)]
    public string Password { get; set; } = default!;
}