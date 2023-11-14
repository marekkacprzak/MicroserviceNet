using Actio.Common;
using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Services.Activities.Exceptions;
using Actio.Services.Identity.Services;
using RawRabbit;

namespace Actio.Services.Identity.Handlers;

public class CreateUserHandler(IBusClient busClient,
    IUserService userService, ILogger<CreateUserHandler> logger) : ICommandHandler<CreateUser>
{
    public async Task HandleAsync(CreateUser command)
    {
        //Console.WriteLine($"Creating user: {command.Email} {command.Name}");
        logger.LogInformation($@"Creating user: {command.Email} {command.Name}");
        try
        {
            await userService.RegisterAsync(command.Email, command.Password, command.Name);
            await busClient.PublishAsync(
                new UserCreated(command.Email, command.Name));
        }
        catch (ActioException ex)
        {
            await busClient.PublishAsync(new CreateUserRejected(ex.Message, ex.Code, command.Email));
        }
        catch (Exception ex)
        {
            await busClient.PublishAsync(new CreateUserRejected(ex.Message, "error", command.Email));
        }
    }
}