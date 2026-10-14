using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SavelevCrossPlatform.Dtos;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponseDto> Login([FromBody] LoginDto request)
    {
        var username = request.Username.Trim();
        var password = _configuration["DemoAccounts:Password"];
        if (username.Length == 0 || string.IsNullOrEmpty(password) || request.Password != password)
            return Unauthorized("Неверный логин или пароль.");

        var role = username.Equals("admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };
        var expiresAt = DateTime.UtcNow.AddHours(2);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        return Ok(new LoginResponseDto(new JwtSecurityTokenHandler().WriteToken(token), role, expiresAt));
    }
}
