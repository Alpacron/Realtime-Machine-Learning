using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class User
{
    [Required]
    public int Id { get; set; }

    [Required, MinLength(4)]
    public string Username { get; set; }

    [Required, JsonIgnore]
    public string PasswordHash { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    public User(string username, string passwordHash, string email)
    {
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
    }
}
