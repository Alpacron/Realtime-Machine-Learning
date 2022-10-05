using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("authenticated")]
    public IActionResult Authenticate()
    {
        return Ok("authenticated");
    }
}
