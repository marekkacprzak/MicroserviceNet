using Actio.Api.Repositories;
using Actio.Common.Events;
using Actio.Api.Models;

namespace Actio.Api.Handlers;

public class ActivityCreatedHandler(IActivityRepository activityRepository,
    ILogger<ActivityCreatedHandler> logger):IEventHandler<ActivityCreated>
{
    public async Task HandleAsync(ActivityCreated @event)
    {
        await activityRepository.AddAsync(new Activity
        {
            Id = @event.Id,
            Name = @event.Name,
            Description = @event.Description,
            Category = @event.Category,
            CreatedAt = @event.CreatedAt,
            UserId = @event.UserId
        });
        await Task.CompletedTask;
        logger.LogInformation($"Activity created: {@event.Name}");
    }
}