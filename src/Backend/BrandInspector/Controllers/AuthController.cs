using BrandInspector.Dtos;
using BrandInspector.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrandInspector.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto request, CancellationToken cancellationToken)
    {
        string token = await authService.Login(request.Username, request.Password, cancellationToken);
        
        return Ok(new { accessToken = token });
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] UserDto request, CancellationToken cancellationToken)
    {
        await authService.SignUp(request.Username, request.Password, cancellationToken);
        return Ok(new { success = true});
    }
}
