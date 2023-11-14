using Actio.Api.Repositories;
using Actio.Common.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Actio.Api.Controllers;

[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ActivitiesController(IBusClient busClient,
    IActivityRepository activityRepository) : Controller
{
    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] CreateActivity command)
    {
        command.Id ??= Guid.NewGuid();
        command.CreatedAt ??= DateTime.UtcNow;
        command.UserId = Guid.Parse(User.Identity?.Name!);
        await busClient.PublishAsync(command);
        return Accepted($"activities/{command.Id}");
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var activities = await activityRepository
            .BrowseAsync(Guid.Parse(User.Identity?.Name!));
        return Json(
            activities.Select(x =>
                new { x.Id, x.Name, x.Category, x.CreatedAt }));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var activity = await activityRepository
            .GetAsync(Guid.Parse(id));
        if (activity.UserId != Guid.Parse(User.Identity?.Name!))
        {
            return Unauthorized();
        }
        return Json(activity);
    }
}