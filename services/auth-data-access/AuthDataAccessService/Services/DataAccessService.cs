using AuthDataAccessService.Data;
using AuthDataAccessService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthDataAccessService.Services;

public interface IDataAccessService
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(int id);
    Task<User> AddUser(User user);
    Task<User?> UpdateUser(User user);
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

    public async Task<User?> UpdateUser(User updateduser)
    {
        var olduser = await _context.User.SingleOrDefaultAsync(m => m.Id == updateduser.Id);
        if (olduser == null)
            return null;

        olduser.Email = updateduser.Email;
        olduser.Username = updateduser.Username;
        olduser.PasswordHash = updateduser.PasswordHash;

        await _context.SaveChangesAsync();

        return updateduser;
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
