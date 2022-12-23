using Microsoft.EntityFrameworkCore;
using AuthService.Models;

namespace AuthService.Data;

public class AuthContext : DbContext
{
    public AuthContext (DbContextOptions<AuthContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;
}
