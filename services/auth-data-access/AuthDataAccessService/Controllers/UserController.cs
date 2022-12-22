using AuthDataAccessService.Services;
using Microsoft.AspNetCore.Mvc;
using AuthDataAccessService.Models;

namespace AuthDataAccessService.Controllers;

[ApiController]
[Route("")]
public class UserController : ControllerBase
{
    private readonly IDataAccessService _dataAccessService;

    public UserController(IDataAccessService dataAccessService)
    {
        _dataAccessService = dataAccessService;
    }

    [HttpGet("getbyemail/{email}")]
    public IActionResult GetByEmail(string email)
    {
        var response = _dataAccessService.GetByEmail(email).Result;

        if (response == null) return NotFound(email);

        return Ok(response);
    }

    [HttpGet("getbyid/{id}")]
    public IActionResult GetById(int id)
    {
        var response = _dataAccessService.GetById(id).Result;

        if(response == null) return NotFound(id);

        return Ok(response);
    }

    [HttpPost("adduser")]
    public IActionResult AddUser(User user)
    {
        var response = _dataAccessService.AddUser(user).Result;

        return Ok(response);
    }

    [HttpPut("updateuser")]
    public IActionResult UpdateUser(User user)
    {
        var response = _dataAccessService.UpdateUser(user).Result;

        if (response == null) return NotFound(user.Id);

        return Ok(response);


    }

    [HttpDelete("deleteuser")]
    public IActionResult DeleteUser(int id)
    {
        var response = _dataAccessService.DeleteUser(id).Result;

        if (response == null) return NotFound(id);

        return Ok(response);
    }
}
