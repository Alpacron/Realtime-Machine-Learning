using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AuthService.Models;

namespace AuthService.Services;

public interface IUserService
{
    string Authenticate(AuthenticateRequest model);
}

public class UserService : IUserService
{
    public string Authenticate(AuthenticateRequest model)
    {
        return "yes";
    }
}
