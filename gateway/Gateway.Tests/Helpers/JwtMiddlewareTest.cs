using Gateway.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gateway.Tests.Helpers;

internal class JwtMiddlewareTest
{
    private string jwtkey;

    [SetUp]
    public void Setup()
    {
        jwtkey = "testkey123456789123456789123456789";
    }

    [Test]
    public void Validate_WithValidToken_ReturnsTrue()
    {
        bool validToken = JwtMiddleware.Validate(generateJwtToken(1), jwtkey);

        Assert.That(validToken, Is.True);
    }

    [Test]
    public void Validate_WithInValidToken_ReturnsTrue()
    {
        bool validToken = JwtMiddleware.Validate("badtoken", jwtkey);

        Assert.That(validToken, Is.False);
    }

    private string generateJwtToken(int id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtkey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
