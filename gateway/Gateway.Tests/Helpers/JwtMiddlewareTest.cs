using Gateway.Helpers;
using KubeClient.Extensions.KubeConfig.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YamlDotNet.Core.Tokens;

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
    public async Task Validate_WithValidToken_ReturnsTrue()
    {
        bool validToken = JwtMiddleware.Validate(generateJwtToken(1), jwtkey);

        Assert.That(validToken, Is.True);
    }

    [Test]
    public async Task Validate_WithInValidToken_ReturnsTrue()
    {
        bool validToken = JwtMiddleware.Validate("badtoken", jwtkey);

        Assert.That(validToken, Is.False);
    }

    public string generateJwtToken(int id)
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
