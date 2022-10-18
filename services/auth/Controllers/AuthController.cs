using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthService.Models;
using AuthService.Services;

namespace AuthService.Controllers;

[ApiController]
[Route("")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("signup")]
    public IActionResult Signup(string username, string password, string email)
    {
        if (username == null || password == null || email == null)
        {
            return NotFound();
        }

        User user = _userService.Create(username, password, email).Result;

        return Ok(user);
    }

    [HttpPost("signin")]
    public IActionResult Login(string email, string password)
    {
        if (password == null || email == null)
        {
            return NotFound();
        }

        User user = _userService.GetByEmail(email).Result;

        bool result = _userService.VerifyPassword(user.Id, password).Result;

        return Ok(user);
    }

    [Authorize]
    [HttpGet("signedin")]
    public IActionResult IsLoggedIn()
    {
        return Ok(true);
    }

    [Authorize]
    [HttpPost("signout")]
    public IActionResult LogOut()
    {
        return Ok(true);
    }

    [Authorize]
    [HttpPost("delete")]
    public IActionResult Delete()
    {
        return Ok(true);
    }
}
