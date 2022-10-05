using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using AuthService.Services;

namespace AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("signedin")]
    public IActionResult LoggedIn()
    {
        return Ok(true);
    }

    [HttpPost("register")]
    public IActionResult CreateUser(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(true);
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate()
    {
        return Ok(true);
    }
}
