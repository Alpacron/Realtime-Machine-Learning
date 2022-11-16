using AuthService.Helpers;
using AuthService.Data;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthService.Services;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest);
    Task<AuthenticateResponse> Register(RegisterRequest registerRequest);
    Task<User> GetById(int id);
    Task<DeleteResponse> Delete(int id);
    Task<AuthenticateResponse> Update(int id, UpdateRequest updateRequest);
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
        User user = await _context.User.SingleOrDefaultAsync(m => m.Email == authenticateRequest.Email);

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

        var token = _jwt.generateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public async Task<AuthenticateResponse> Register(RegisterRequest registerRequest)
    {
        User existingUser = await _context.User.SingleOrDefaultAsync(m => m.Email == registerRequest.Email);
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

        _context.Add(user);

        var token = _jwt.generateJwtToken(user);

        await _context.SaveChangesAsync();

        return new AuthenticateResponse(user, token);
    }

    public async Task<User> GetById(int id)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

        return user;
    }

    public async Task<DeleteResponse> Delete(int id)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return new DeleteResponse()
            {
                Success = false,
                Details = "User does not exist."
            };
        }

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return new DeleteResponse(user);
    }

    public async Task<AuthenticateResponse> Update(int id, UpdateRequest updateRequest)
    {
        var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return new AuthenticateResponse()
            {
                Success = false,
                Details = "User does not exist."
            };
        }

        if (updateRequest.Email != null)
            user.Email = updateRequest.Email;
        if (updateRequest.Username != null)
            user.Username = updateRequest.Username;
        if (updateRequest.Password != null)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateRequest.Password);

        var token = _jwt.generateJwtToken(user);

        await _context.SaveChangesAsync();

        return new AuthenticateResponse(user, token);
    }
}
