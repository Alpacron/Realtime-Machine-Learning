using AuthService.Models;
using AuthService.Services;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.Tests.Services;

internal class CacheServiceTest
{
    private readonly User defaultUser = new()
    {
        Id = 1,
        Email = "test@test.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
        Username = "test"
    };

    [Test]
    public void Test_Caching()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        CacheService cacheService = new(cache);

        cacheService.CacheSet(defaultUser);

        cacheService.CacheGet(defaultUser.Email, out User? user);

        Assert.That(user!.Id, Is.EqualTo(defaultUser.Id));

        cacheService.CacheRemove(defaultUser);

        cacheService.CacheGet(defaultUser.Id, out User? userB);

        Assert.That(userB, Is.Null);
    }
}
