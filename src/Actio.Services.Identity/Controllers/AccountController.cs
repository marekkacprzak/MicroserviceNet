using Actio.Common.Commands;
using Actio.Services.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Actio.Services.Identity.Controllers;

[Route("")]
public class AccountController(IUserService userService):Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationUser command)
        => Json(await userService.LoginAsync(command.Email, command.Password));
}