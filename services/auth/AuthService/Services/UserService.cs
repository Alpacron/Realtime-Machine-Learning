using AuthService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest);
    Task<AuthenticateResponse> Register(RegisterRequest registerRequest);
    Task<DeleteResponse> Delete(int id);
    Task<AuthenticateResponse> Update(int id, UpdateRequest updateRequest);
}

public class UserService : IUserService
{
    private readonly string _jwtSecret;
    private readonly IDataAccessService _dataAccessService;

    public UserService(IConfiguration configuration, IDataAccessService dataAccessService)
    {
        _jwtSecret = configuration["Jwt:Secret"];
        _dataAccessService = dataAccessService;
    }
    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
    {
        User? user = await _dataAccessService.GetByEmail(authenticateRequest.Email);

        if (user == null) return new AuthenticateResponse()
        {
            Success = false,
            Details = "Email or password is incorrect."
        };

        bool verify = BCrypt.Net.BCrypt.Verify(authenticateRequest.Password, user.PasswordHash);
        if (!verify) return new AuthenticateResponse()
        {
            Success = false,
            Details = "Email or password is incorrect."
        };

        var token = GenerateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public async Task<AuthenticateResponse> Register(RegisterRequest registerRequest)
    {
        User? existingUser = await _dataAccessService.GetByEmail(registerRequest.Email);
        if (existingUser != null) return new AuthenticateResponse()
        {
            Success = false,
            Details = "Email is already in use."
        };

        var user = new User()
        {
            Email = registerRequest.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
            Username = registerRequest.Username
        };

        await _dataAccessService.AddUser(user);

        var token = GenerateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public async Task<DeleteResponse> Delete(int id)
    {
        User? user = await _dataAccessService.DeleteUser(id);
        if (user == null)
        {
            return new DeleteResponse()
            {
                Success = false,
                Details = "User does not exist."
            };
        }

        return new DeleteResponse(user);
    }

    public async Task<AuthenticateResponse> Update(int id, UpdateRequest updateRequest)
    {
        User? existingUser = await _dataAccessService.GetById(id);
        if (existingUser is null)
        {
            return new AuthenticateResponse()
            {
                Success = false,
                Details = "User does not exist."
            };
        }

        User newUser = new()
        {
            Id = id,
            Email = updateRequest.Email is string e? e : existingUser.Email,
            Username = updateRequest.Username is string u ? u : existingUser.Username,
            PasswordHash = updateRequest.Password is string p ? BCrypt.Net.BCrypt.HashPassword(p) : existingUser.PasswordHash,
        };

        User? updatedUser = await _dataAccessService.UpdateUser(newUser);
        if (updatedUser is null)
        {
            return new AuthenticateResponse()
            {
                Success = false,
                Details = "User does not exist."
            };
        }

        var token = GenerateJwtToken(updatedUser);

        return new AuthenticateResponse(updatedUser, token);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
