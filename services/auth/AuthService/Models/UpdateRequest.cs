using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class UpdateRequest
{
    [EmailAddress]
    public string? Email { get; set; }

    [MinLength(4)]
    public string? Username { get; set; }

    [MinLength(4)]
    public string? Password { get; set; }
}