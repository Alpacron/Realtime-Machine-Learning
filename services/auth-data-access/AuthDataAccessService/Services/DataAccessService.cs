using AuthDataAccessService.Data;
using AuthDataAccessService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthDataAccessService.Services;

public interface IDataAccessService
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(int id);
    Task<User> AddUser(User user);
    Task<User?> UpdateUser(User updatedUser);
    Task<User?> DeleteUser(int id);
}

public class DataAccessService : IDataAccessService
{
    private readonly AuthContext _context;

    public DataAccessService(AuthContext authContext)
    {
        _context = authContext;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Email == email);
        return user;
    }

    public async Task<User?> GetById(int id)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
        return user;
    }

    public async Task<User> AddUser(User user)
    {
        _context.Add(user);
        await _context.SaveChangesAsync();
        
        return user;
    }

    public async Task<User?> UpdateUser(User updatedUser)
    {
        var olduser = await _context.User.SingleOrDefaultAsync(m => m.Id == updatedUser.Id);
        if (olduser == null)
            return null;

        olduser.Email = updatedUser.Email;
        olduser.Username = updatedUser.Username;
        olduser.PasswordHash = updatedUser.PasswordHash;

        await _context.SaveChangesAsync();

        return updatedUser;
    }

    public async Task<User?> DeleteUser(int id)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
        if (user == null)
            return null;

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
