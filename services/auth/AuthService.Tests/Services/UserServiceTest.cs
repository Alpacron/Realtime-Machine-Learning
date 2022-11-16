using AuthService.Services;
using AuthService.Models;
using AuthService.Helpers;
using AuthService.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace authservice.Tests;

public class UserServiceTest
{
    private Jwt jwt;
    private readonly User defaultUser = new()
    {
        Id = 0,
        Email = "test@test.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
        Username = "test"
    };
    private UserService userService;
    private AuthContext context;

    [SetUp]
    public void Setup()
    {
        jwt = new() { Key = "testkey123456789123456789123456789" };

        DbContextOptions<AuthContext> options = new DbContextOptionsBuilder<AuthContext>()
            .UseInMemoryDatabase(databaseName: "UserDb: " + Guid.NewGuid().ToString())
            .Options;
        context = new AuthContext(options);
        context.User.Add(defaultUser);
        context.SaveChanges();
        userService = new UserService(context, Options.Create(jwt));
    }

    [TearDown]
    public void Teardown()
    {
        context.Dispose();
    }

    [Test]
    public async Task Authenticate_ValidAuth_ReturnsUser()
    {
        AuthenticateRequest request = new()
        {
            Email = "test@test.com",
            Password = "Test123!"
        };

        AuthenticateResponse response = await userService.Authenticate(request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Id, Is.EqualTo(defaultUser.Id));
        });
    }

    [Test]
    public async Task Authenticate_InvalidEmail_ReturnsNull()
    {
        AuthenticateRequest request = new()
        {
            Email = "BadEmail",
            Password = "Test123!"
        };

        AuthenticateResponse response = await userService.Authenticate(request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.False);
            Assert.That(response.Result, Is.Null);
        });
    }

    [Test]
    public async Task Authenticate_InvalidPassword_ReturnsNull()
    {
        AuthenticateRequest request = new()
        {
            Email = "test@test.com",
            Password = "BadPassword"
        };

        AuthenticateResponse response = await userService.Authenticate(request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.False);
            Assert.That(response.Result, Is.Null);
        });
    }

    [Test]
    public async Task Reqister_UsedEmail_ReturnsNull()
    {
        RegisterRequest request = new()
        {
            Email = "test@test.com",
            Username = "test",
            Password = "Test123!"
        };

        AuthenticateResponse response = await userService.Register(request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.False);
            Assert.That(response.Result, Is.Null);
        });
    }

    [Test]
    public async Task Reqister_NewUser_ReturnsUser()
    {
        RegisterRequest request = new()
        {
            Email = "test2@test.com",
            Username = "test2",
            Password = "Test123!"
        };

        AuthenticateResponse response = await userService.Register(request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Username, Is.EqualTo("test2"));
        });
    }

    [Test]
    public async Task Authenticate_NewlyRegisteredUser_ReturnsUser()
    {
        RegisterRequest request = new()
        {
            Email = "test2@test.com",
            Username = "test2",
            Password = "Test123!"
        };

        await userService.Register(request);

        AuthenticateResponse response = await userService.Authenticate(new AuthenticateRequest()
        {
            Email = "test2@test.com",
            Password = "Test123!"
        });

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Username, Is.EqualTo("test2"));
        });
    }

    [Test]
    public async Task Delete_ValidUser_DeletesUser()
    {
        DeleteResponse response = await userService.Delete(defaultUser.Id);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Id, Is.EqualTo(defaultUser.Id));
            Assert.That(context.User.FirstOrDefault(), Is.Null);
        });
    }

    [Test]
    public async Task Delete_UnknownUser_ReturnsNull()
    {
        int userId = 1;

        DeleteResponse response = await userService.Delete(userId);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.False);
            Assert.That(response.Result, Is.Null);
        });
    }

    [Test]
    public async Task GetById_ValidUser_ReturnsUser()
    {
        User user = await userService.GetById(defaultUser.Id);

        Assert.That(user, Is.EqualTo(defaultUser));
    }

    [Test]
    public async Task GetById_UnkownUser_ReturnsNull()
    {
        int userId = 1;

        User user = await userService.GetById(userId);

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task Update_ValidUser_Username_UpdatesUsername()
    {
        UpdateRequest request = new UpdateRequest() { Username = "test2" };
        AuthenticateResponse response = await userService.Update(defaultUser.Id, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Id, Is.EqualTo(defaultUser.Id));
            Assert.That(context.User.FirstOrDefault().Username, Is.EqualTo(request.Username));
        });
    }

    [Test]
    public async Task Update_ValidUser_Email_UpdatesEmail()
    {
        UpdateRequest request = new UpdateRequest() { Email = "test2@test.com" };
        AuthenticateResponse response = await userService.Update(defaultUser.Id, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Id, Is.EqualTo(defaultUser.Id));
            Assert.That(context.User.FirstOrDefault().Email, Is.EqualTo(request.Email));
        });
    }

    [Test]
    public async Task Update_ValidUser_Password_UpdatesPassword()
    {
        UpdateRequest request = new UpdateRequest() { Password = "NewTestPassword123!" };
        AuthenticateResponse response = await userService.Update(defaultUser.Id, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Id, Is.EqualTo(defaultUser.Id));
            Assert.That(BCrypt.Net.BCrypt.Verify(context.User.FirstOrDefault().PasswordHash, defaultUser.PasswordHash), Is.False);
        });
    }

    [Test]
    public async Task Update_ValidUser_AllValues_UpdatesAllValues()
    {
        UpdateRequest request = new UpdateRequest() {
            Username = "test2",
            Email = "test2@test.com",
            Password = "NewTestPassword123!"
        };
        AuthenticateResponse response = await userService.Update(defaultUser.Id, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.Id, Is.EqualTo(defaultUser.Id));
            Assert.That(context.User.FirstOrDefault().Username, Is.EqualTo(request.Username));
            Assert.That(context.User.FirstOrDefault().Email, Is.EqualTo(request.Email));
            Assert.That(BCrypt.Net.BCrypt.Verify(context.User.FirstOrDefault().PasswordHash, defaultUser.PasswordHash), Is.False);
        });
    }

    [Test]
    public async Task Update_UnkownUser_ReturnsNull()
    {
        UpdateRequest request = new UpdateRequest() { Username = "test2" };
        int userId = 1;
        AuthenticateResponse response = await userService.Update(userId, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.False);
            Assert.That(response.Result, Is.Null);
        });
    }
}