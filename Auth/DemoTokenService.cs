using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace SavelevCrossPlatform.Auth;

public class DemoTokenService
{
    private const string Issuer = "SavelevCrossPlatform";
    private const string Audience = "SavelevCrossPlatform.Api";
    private readonly SymmetricSecurityKey _signingKey = new(RandomNumberGenerator.GetBytes(32));

    public TokenValidationParameters ValidationParameters => new()
    {
        ValidateIssuer = true,
        ValidIssuer = Issuer,
        ValidateAudience = true,
        ValidAudience = Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = _signingKey,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };

    public object CreateToken(string role)
    {
        var expires = DateTime.UtcNow.AddHours(1);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, role == "Admin" ? "demo-admin" : "demo-user"),
            new Claim(ClaimTypes.Role, role)
        };
        var jwt = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256));

        return new
        {
            token = new JwtSecurityTokenHandler().WriteToken(jwt),
            role,
            expiresAt = expires
        };
    }
}
