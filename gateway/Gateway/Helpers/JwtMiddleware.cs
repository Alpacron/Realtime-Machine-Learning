using Microsoft.IdentityModel.Tokens;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Gateway.Helpers;
public static class JwtMiddleware
{
    public static Func<HttpContext, string, Func<Task>, Task> CreateAuthorizationFilter
        => async (context, securityKey, next) =>
        {
            var routeClaimsRequirements = context.Items.DownstreamRoute().RouteClaimsRequirement;
            if (routeClaimsRequirements.Count > 0)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null && Validate(routeClaimsRequirements, token, securityKey))
                {
                    await next.Invoke();
                }
            }
            else
            {
                await next.Invoke();
            }
        };

    public static bool Validate(Dictionary<string, string> routeClaimsRequirements, string token, string securityKey)
    {
        // validate token
        try
        {
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

