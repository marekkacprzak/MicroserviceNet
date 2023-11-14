using Actio.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Actio.Api.Controllers;

[Route("[controller]")]
public class UsersController(IBusClient busClient) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Post([FromBody] CreateUser command)
    {
        await busClient.PublishAsync(command);
        return Accepted();
    }
}