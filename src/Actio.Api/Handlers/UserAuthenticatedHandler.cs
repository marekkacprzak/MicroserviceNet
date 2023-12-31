using Actio.Common.Events;

namespace Actio.Api.Handlers;

public class UserAuthenticatedHandler:IEventHandler<UserAuthenticated>
{
    public async Task HandleAsync(UserAuthenticated @event)
    {
        await Task.CompletedTask;
        Console.WriteLine($"User Authenticated: {@event.Email}");
    }
}