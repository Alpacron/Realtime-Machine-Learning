using AuthService.Data;
using AuthService.Models;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace AuthService.Tests.Services;

internal class DataAccessServiceTest
{
    private DataAccessService dataAccessService;
    private Mock<IMessagingService> messagingService;
    private AuthContext context;
    private IMemoryCache cache;

    private readonly User defaultUser = new()
    {
        Id = 1,
        Email = "test@test.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
        Username = "test"
    };

    [SetUp]
    public void Setup()
    {
        DbContextOptions<AuthContext> options = new DbContextOptionsBuilder<AuthContext>()
            .UseInMemoryDatabase(databaseName: "UserDb: " + Guid.NewGuid().ToString())
            .Options;
        context = new AuthContext(options);
        context.User.Add(defaultUser);
        context.SaveChanges();
        cache = new MemoryCache(new MemoryCacheOptions());
        messagingService = new Mock<IMessagingService>();
        dataAccessService = new(context, cache, messagingService.Object);
    }
}
