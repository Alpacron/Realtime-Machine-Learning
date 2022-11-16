using AuthService.Controllers;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace authservice.Tests.Controllers;

public class AuthControllerTest
{
    private readonly User defaultUser = new()
    {
        Id = 0,
        Email = "test@test.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
        Username = "test"
    };
    private readonly string token = "bearer test token";

    private Mock<IUserService> userService;

    [SetUp]
    public void Setup()
    {
        userService = new Mock<IUserService>();
    }

    [Test]
    public void Authenticate_ValidResponse_ReturnsStatusOk()
    {
        AuthenticateRequest request = new();
        userService.Setup(x => x.Authenticate(request)).Returns(Task.FromResult(new AuthenticateResponse(defaultUser, "test token")));
        AuthController authController = new(userService.Object);

        IActionResult result = authController.Authenticate(request);
        var okResult = result as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public void Authenticate_NotFoundResponse_ReturnsStatusOk()
    {
        AuthenticateRequest request = new();
        userService.Setup(x => x.Authenticate(request)).Returns(Task.FromResult(new AuthenticateResponse() { Success = false }));
        AuthController authController = new(userService.Object);

        IActionResult result = authController.Authenticate(request);
        var notFoundResult = result as NotFoundObjectResult;


        Assert.Multiple(() =>
        {
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        });
    }

    [Test]
    public void Register_ValidResponse_ReturnsStatusOk()
    {
        RegisterRequest request = new();
        userService.Setup(x => x.Register(request)).Returns(Task.FromResult(new AuthenticateResponse(defaultUser, token)));
        AuthController authController = new(userService.Object);

        IActionResult result = authController.Register(request);
        var okResult = result as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public void Register_NotFoundResponse_ReturnsStatusOk()
    {
        RegisterRequest request = new();
        userService.Setup(x => x.Register(request)).Returns(Task.FromResult(new AuthenticateResponse() { Success = false }));
        AuthController authController = new(userService.Object);

        IActionResult result = authController.Register(request);
        var notFoundResult = result as BadRequestObjectResult;


        Assert.Multiple(() =>
        {
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public void Authenticated_WithUser_ReturnsUser()
    {
        AuthController authController = new(userService.Object);
        authController.ControllerContext.HttpContext = new DefaultHttpContext();
        authController.ControllerContext.HttpContext.Items["User"] = defaultUser;
        authController.ControllerContext.HttpContext.Request.Headers.Authorization = token;

        IActionResult result = authController.Authenticated();
        var okResult = result as OkObjectResult;


        Assert.Multiple(() =>
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public void Delete_ValidResponse_ReturnsStatusOk()
    {
        userService.Setup(x => x.Delete(defaultUser.Id)).Returns(Task.FromResult(new DeleteResponse(defaultUser)));
        AuthController authController = new(userService.Object);
        authController.ControllerContext.HttpContext = new DefaultHttpContext();
        authController.ControllerContext.HttpContext.Items["User"] = defaultUser;

        IActionResult result = authController.Delete();
        var okResult = result as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public void Delete_NotFoundResponse_ReturnsNotFound()
    {
        userService.Setup(x => x.Delete(defaultUser.Id)).Returns(Task.FromResult(new DeleteResponse() { Success = false }));
        AuthController authController = new(userService.Object);
        authController.ControllerContext.HttpContext = new DefaultHttpContext();
        authController.ControllerContext.HttpContext.Items["User"] = defaultUser;

        IActionResult result = authController.Delete();
        var notFoundResult = result as NotFoundObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        });
    }

    [Test]
    public void Update_NewUsername_UpdatesUsername()
    {
        UpdateRequest request = new()
        {
            Username = "test2"
        };
        userService.Setup(x => x.Update(defaultUser.Id, request)).Returns(Task.FromResult(new AuthenticateResponse(defaultUser, token)));
        AuthController authController = new(userService.Object);
        authController.ControllerContext.HttpContext = new DefaultHttpContext();
        authController.ControllerContext.HttpContext.Items["User"] = defaultUser;

        IActionResult result = authController.Update(request);
        var okResult = result as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public void Update_NotFoundResponse_ReturnsNotFound()
    {
        UpdateRequest request = new()
        {
            Username = "test2"
        };
        userService.Setup(x => x.Update(defaultUser.Id, request)).Returns(Task.FromResult(new AuthenticateResponse() { Success = false }));
        AuthController authController = new(userService.Object);
        authController.ControllerContext.HttpContext = new DefaultHttpContext();
        authController.ControllerContext.HttpContext.Items["User"] = defaultUser;

        IActionResult result = authController.Update(request);
        var notFoundResult = result as NotFoundObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        });
    }
}