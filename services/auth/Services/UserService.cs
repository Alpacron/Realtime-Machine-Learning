using AuthService.Data;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services;

public interface IUserService
{
    Task<User> Create(string username, string password, string email);
    Task<User> GetByEmail(string email);
    Task<bool> VerifyPassword(int id, string password);
    Task<User> Delete(int id);
}

public class UserService : IUserService
{
    private readonly AuthContext _context;

    public UserService(AuthContext context)
    {
        _context = context;
    }

    public async Task<User> Create(string username, string password, string email)
    {
        if (await UserWithEmailExists(email)) throw new Exception("email was already taken.");

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Email = email
        };

        _context.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> UserWithEmailExists(string email)
    {
        var user = await _context.User.FirstOrDefaultAsync(m => m.Email == email);

        return user != null;
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _context.User.FirstOrDefaultAsync(m => m.Email == email);

        if (user == null)
        {
            throw new KeyNotFoundException("User with mail not found.");
        }

        return user;
    }

    public async Task<bool> VerifyPassword(int id, string password)
    {
        var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

        if (user == null)
        {
            throw new KeyNotFoundException("User with id not found.");
        }

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<User> Delete(int id)
    {
        var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

        if (user == null)
        {
            throw new KeyNotFoundException("User with id not found.");
        }

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
