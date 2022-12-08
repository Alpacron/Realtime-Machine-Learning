using Microsoft.IdentityModel.Tokens;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace Gateway.Helpers;
public static class JwtMiddleware
{
    public static Func<HttpContext, string, Func<Task>, Task> CreateAuthorizationFilter
        => async (context, securityKey, next) =>
        {
            if (bool.Parse(context.Items.DownstreamRoute().RouteClaimsRequirement["Auth"]) == true)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null && Validate(token, securityKey))
                {
                    await next.Invoke();
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Items.SetError(new UnauthenticatedError("Unauthorized"));
                }
            }
            else
            {
                await next.Invoke();
            }
        };

    public static bool Validate(string token, string securityKey)
    {
        try
        {
            // validate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken != null;
        } catch
        {
            return false;
        }

    }
}

