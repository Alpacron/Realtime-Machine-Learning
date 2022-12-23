using AuthService.Data;
using AuthService.Models;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace AuthService.Tests.Services;

internal class DataAccessServiceTest
{
    //private DataAccessService dataAccessService;
    //private Mock<IMessagingService> messagingService;
    //private AuthContext context;

    //private readonly User defaultUser = new()
    //{
    //    Id = 1,
    //    Email = "test@test.com",
    //    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
    //    Username = "test"
    //};

    //[SetUp]
    //public void Setup()
    //{
    //    DbContextOptions<AuthContext> options = new DbContextOptionsBuilder<AuthContext>()
    //        .UseInMemoryDatabase(databaseName: "UserDb: " + Guid.NewGuid().ToString())
    //        .Options;
    //    context = new AuthContext(options);
    //    context.User.Add(defaultUser);
    //    context.SaveChanges();
    //    messagingService = new Mock<IMessagingService>();
    //    dataAccessService = new(context, messagingService.Object, );
    //}

    //[Test]
    //public async Task Test_Caching()
    //{
    //    var user = await dataAccessService.GetByEmail(defaultUser.Email);

    //    Assert.That(user, Is.Not.Null);
    //    Assert.That(user.Id, Is.EqualTo(defaultUser.Id));


    //    cache.TryGetValue(user.Id, out User u);
    //    cache.TryGetValue(user.Email, out int id);
    //    Assert.That(id, Is.EqualTo(defaultUser.Id));
    //    Assert.That(u.Id, Is.EqualTo(defaultUser.Id));
    //}
}
