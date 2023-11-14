using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Services.Activities.Exceptions;
using Actio.Services.Activities.Services;
using RawRabbit;

namespace Actio.Services.Activities.Handlers;

public class CreateActivityHandler(
    IBusClient busClient, 
    IActivityService activityService,
    ILogger<CreateActivityHandler> logger):ICommandHandler<CreateActivity>
{ 
    public async Task HandleAsync(CreateActivity command)
    {
        logger.LogInformation($@"Creating activity:  {command.Category} {command.Name}");
        var id = command.Id ?? Guid.NewGuid();
        try
        {
            await activityService.AddAsync(command.Id!.Value, command.UserId, command.Category, command.Name,
                command.Description, command.CreatedAt!.Value);
            await busClient.PublishAsync(new ActivityCreated(
                id,
                command.UserId,
                command.Category,
                command.Name,
                command.Description,
                command.CreatedAt ?? DateTime.UtcNow));
        }
        catch (ActioException ex)
        {
            await busClient.PublishAsync(new CreateActivityRejected(id, ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            await busClient.PublishAsync(new CreateActivityRejected(id,  "error", ex.Message));

        }
    }
}