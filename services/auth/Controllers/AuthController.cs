using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using AuthService.Services;
using AuthService.Helpers;
using System.Collections.Generic;

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

    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest authenticateRequest)
    {
        AuthenticateResponse response = _userService.Authenticate(authenticateRequest).Result;

        if (response == null)
        {
            return BadRequest(new ProblemDetails()
            {
                Title = "Email or password is incorrect."
            });
        }

        return Ok(response);
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        AuthenticateResponse response = _userService.Register(registerRequest).Result;

        if (response == null)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            errors.Add("Email", new string[] { "Email is already in use." });
            ValidationProblemDetails problem = new ValidationProblemDetails(errors);
            return BadRequest(problem);
        }

        return Ok(response);
    }

    [Authorize]
    [HttpGet("authenticated")]
    public IActionResult Authenticated()
    {
        return Ok();
    }

    [Authorize]
    [HttpPost("signout")]
    public IActionResult Signout()
    {
        return Ok();
    }

    [Authorize]
    [HttpPost("delete")]
    public IActionResult Delete()
    {
        return Ok();
    }
}
