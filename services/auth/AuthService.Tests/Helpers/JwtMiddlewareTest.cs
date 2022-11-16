using AuthService.Helpers;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace authservice.Tests.Helpers;

public class JwtMiddlewareTest
{
    private readonly User defaultUser = new()
    {
        Id = 0,
        Email = "test@test.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
        Username = "test"
    };

    private JwtMiddleware jwtMiddleware;
    private Mock<IUserService> userService;
    private HttpContext context;
    private Jwt jwt;

    [SetUp]
    public void Setup()
    {
        jwt = new() { Key = "testkey123456789123456789123456789" };
        RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
        jwtMiddleware = new(next, Options.Create(jwt));
        context = new DefaultHttpContext();
        userService = new();
        userService.Setup(x => x.GetById(defaultUser.Id)).Returns(Task.FromResult(defaultUser));
    }

    [Test]
    public async Task Invoke_WithToken_AttachesUserToContext()
    {
        context.Request.Headers.Authorization = jwt.generateJwtToken(defaultUser);
        await jwtMiddleware.Invoke(context, userService.Object);

        Assert.That(context.Items["User"], Is.EqualTo(defaultUser));
    }

    [Test]
    public async Task Invoke_WithoutToken_ReturnsNull()
    {
        await jwtMiddleware.Invoke(context, userService.Object);

        Assert.That(context.Items["User"], Is.Null);
    }

    [Test]
    public async Task Invoke_WithInvalidToken_ReturnsNull()
    {
        string token = jwt.generateJwtToken(defaultUser);
        context.Request.Headers.Authorization = token.Remove(token.Length - 1, 1);
        await jwtMiddleware.Invoke(context, userService.Object);

        Assert.That(context.Items["User"], Is.Null);
    }
}