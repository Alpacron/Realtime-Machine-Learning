using AuthDataAccessService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthDataAccessService.Data;

public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options)
    : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;
}
