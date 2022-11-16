using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(4)]
    public string Username { get; set; }

    [Required, MinLength(4)]
    public string Password { get; set; }
}