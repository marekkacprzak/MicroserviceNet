using Actio.Common.Events;

namespace Actio.Api.Handlers;

public class UserCreatedHandler:IEventHandler<UserCreated>
{
    public async Task HandleAsync(UserCreated @event)
    {
        await Task.CompletedTask;
        Console.WriteLine($"User Created: {@event.Name} {@event.Email}");
     }
}