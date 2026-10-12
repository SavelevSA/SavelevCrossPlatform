using Microsoft.AspNetCore.Mvc;
using SavelevCrossPlatform.Auth;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly DemoTokenService _tokens;

    public AuthController(DemoTokenService tokens)
    {
        _tokens = tokens;
    }

    [HttpPost("token/user")]
    public IActionResult UserToken() => Ok(_tokens.CreateToken("User"));

    [HttpPost("token/admin")]
    public IActionResult AdminToken() => Ok(_tokens.CreateToken("Admin"));
}
