namespace AuthService.Models;

public class AuthenticateResult
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}

public class AuthenticateResponse : Response<AuthenticateResult>
{
    public AuthenticateResponse() { }
    public AuthenticateResponse(User user, string token)
    {
        Result = new AuthenticateResult()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = token
        };
    }
}