using AuthService.Data;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace AuthService.Services;

public interface IDataAccessService
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(int id);
    Task<User> AddUser(User user);
    Task<User?> UpdateUser(User updatedUser);
    User? UpdateCachedUser(User updatedUser);
    Task<User?> DeleteUser(int id);
    User? DeleteCachedUser(int id);
}

public class DataAccessService : IDataAccessService
{
    private readonly string AUTH_EXCHANGE = "auth";

    private readonly AuthContext _context;
    private readonly IMessagingService _messagingService;
    private readonly ICacheService _cacheService;

    public DataAccessService(AuthContext authContext, IMessagingService messagingService, ICacheService cacheService)
    {
        _context = authContext;
        _messagingService = messagingService;
        _cacheService = cacheService;
    }

    public async Task<User?> GetByEmail(string email)
    {
        if (!_cacheService.CacheGet(email, out User? user))
        {
            user = await _context.User.SingleOrDefaultAsync(m => m.Email == email);

            if (user != null)
                _cacheService.CacheSet(user);
        }

        return user;
    }

    public async Task<User?> GetById(int id)
    {
        if (!_cacheService.CacheGet(id, out User? user))
        {
            user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (user != null)
                _cacheService.CacheSet(user);
        }

        return user;
    }

    public async Task<User> AddUser(User user)
    {
        _context.Add(user);
        await _context.SaveChangesAsync();

        _cacheService.CacheSet(user);

        return user;
    }

    public async Task<User?> UpdateUser(User updatedUser)
    {
        User? olduser = await GetById(updatedUser.Id);
        if (olduser is null)
            return null;

        olduser.Email = updatedUser.Email;
        olduser.Username = updatedUser.Username;
        olduser.PasswordHash = updatedUser.PasswordHash;

        await _context.SaveChangesAsync();

        var json = JsonConvert.SerializeObject(updatedUser);
        byte[] message = Encoding.UTF8.GetBytes(json);
        _messagingService.Publish(AUTH_EXCHANGE, "", "updateuser", message: message);

        return updatedUser;
    }

    public User? UpdateCachedUser(User updatedUser)
    {
        _cacheService.CacheGet(updatedUser.Id, out User? user);
        if (user is null)
            return null;

        user.Email = updatedUser.Email;
        user.Username = updatedUser.Username;
        user.PasswordHash = updatedUser.PasswordHash;

        _cacheService.CacheRemove(updatedUser);
        _cacheService.CacheSet(user);

        return user;
    }

    public async Task<User?> DeleteUser(int id)
    {
        User? user = await GetById(id);
        if (user is null)
            return null;

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        var json = JsonConvert.SerializeObject(id);
        byte[] message = Encoding.UTF8.GetBytes(json);
        _messagingService.Publish(AUTH_EXCHANGE, "", "deleteuser", message: message);

        return user;
    }

    public User? DeleteCachedUser(int id)
    {
        _cacheService.CacheGet(id, out User? user);
        if (user is null)
            return null;

        _cacheService.CacheRemove(user);

        return user;
    }
}
