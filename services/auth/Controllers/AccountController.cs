using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Controllers;

[ApiController]
[Route("")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("signup")]
    public IActionResult CreateUser(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(true);
    }

    [HttpPost("signin")]
    public IActionResult Login()
    {
        return Ok(true);
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
}
