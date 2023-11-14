using Actio.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Actio.Api.Controllers;
 
[Route("[controller]")]
public class HomeController(IBusClient busClient) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Post([FromBody] CreateUser command)
    {
        await busClient.PublishAsync(command);
        return Accepted();
    }

    [HttpGet("")] 
    public async Task<IActionResult> Get() => await Task.FromResult(Content("Hello from Api"));
}