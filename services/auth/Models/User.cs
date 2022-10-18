using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class User
{
    [Required]
    
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Email { get; set; }
}
