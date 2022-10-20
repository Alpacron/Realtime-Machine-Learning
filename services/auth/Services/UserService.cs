using AuthService.Data;
using AuthService.Helpers;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest);
    Task<AuthenticateResponse> Register(RegisterRequest registerRequest);
    Task<User> GetById(int id);
    Task<User> Delete(int id);
}

public class UserService : IUserService
{
    private readonly AuthContext _context;
    private readonly Jwt _jwt;

    public UserService(AuthContext context, IOptions<Jwt> jwt)
    {
        _context = context;
        _jwt = jwt.Value;
    }
    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Email == authenticateRequest.Email);
        if (user == null) return null;

        bool verify = BCrypt.Net.BCrypt.Verify(authenticateRequest.Password, user.PasswordHash);
        if (!verify) return null;

        var token = generateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public async Task<AuthenticateResponse> Register(RegisterRequest registerRequest)
    {
        var existingUser = await _context.User.SingleOrDefaultAsync(m => m.Email == registerRequest.Email);
        if (existingUser != null) return null;

        var user = new User(registerRequest.Username, BCrypt.Net.BCrypt.HashPassword(registerRequest.Password), registerRequest.Email);

        _context.Add(user);
        await _context.SaveChangesAsync();

        var token = generateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public async Task<User> GetById(int id)
    {
        return await _context.User.SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<User> Delete(int id)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
        if (user == null) return null;

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }

    private string generateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwt.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
