using AuthService.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.Services;

public interface ICacheService
{
    void CacheSet(User user);
    void CacheRemove(User user);
    bool CacheGet(int id, out User? user);
    bool CacheGet(string email, out User? user);
}

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
    }

    public void CacheSet(User user)
    {
        _cache.Set(user.Id, user);
        _cache.Set(user.Email, user.Id);
    }

    public void CacheRemove(User user)
    {
        _cache.Remove(user.Id);
        _cache.Remove(user.Email);
    }

    public bool CacheGet(int id, out User? user)
    {
        _cache.TryGetValue(id, out User? u);
        user = u is not null? u : default;
        return user is not null;
    }

    public bool CacheGet(string email, out User? user)
    {
        _cache.TryGetValue(email, out int id);
        user = CacheGet(id, out User? u) && u is not null ? u : null;
        return user is not null;
    }
}
