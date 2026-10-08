using Microsoft.AspNetCore.Mvc;
using SavelevCrossPlatform.Auth;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(DemoTokenService tokens) : ControllerBase
{
    [HttpPost("token/user")]
    public IActionResult UserToken() => Ok(tokens.CreateToken("User"));

    [HttpPost("token/admin")]
    public IActionResult AdminToken() => Ok(tokens.CreateToken("Admin"));
}
