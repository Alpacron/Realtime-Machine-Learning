using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using AuthService.Services;
using AuthService.Helpers;

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
        var response = _userService.Authenticate(authenticateRequest).Result;

        if (!response.Success)
        {
            return NotFound(new ProblemDetails()
            {
                Title = response.Details
            });
        }

        return Ok(response.Result);
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        AuthenticateResponse response = _userService.Register(registerRequest).Result;

        if (!response.Success)
        {
            Dictionary<string, string[]> errors = new();
            errors.Add("Email", new string[] { response.Details });
            ValidationProblemDetails problem = new(errors);
            return BadRequest(problem);
        }

        return Ok(response.Result);
    }

    [Authorize]
    [HttpGet("authenticated")]
    public IActionResult Authenticated()
    {
        User user = (User)HttpContext.Items["User"];
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        AuthenticateResponse response = new(user, token);

        return Ok(response.Result);
    }

    [Authorize]
    [HttpDelete("delete")]
    public IActionResult Delete()
    {
        User user = (User)HttpContext.Items["User"];

        DeleteResponse response = _userService.Delete(user.Id).Result;

        if (!response.Success)
        {
            return NotFound(new ProblemDetails()
            {
                Title = response.Details
            });
        }

        return Ok(response.Result);
    }

    [Authorize]
    [HttpPut("update")]
    public IActionResult Update(UpdateRequest updateRequest)
    {
        User user = (User)HttpContext.Items["User"];

        AuthenticateResponse response = _userService.Update(user.Id, updateRequest).Result;

        if (!response.Success)
        {
            return NotFound(new ProblemDetails()
            {
                Title = response.Details
            });
        }

        return Ok(response.Result);
    }
}
