using Actio.Common.Events;
using Actio.Common.Services;
using Actio.Common.Servoces;

namespace Actio.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        await ServiceHost.Create<Startup>(args)
            .UseRabbitMq()
            .SubscribeToEvent<ActivityCreated>()
            .SubscribeToEvent<UserAuthenticated>()
            .SubscribeToEvent<UserCreated>()
            .Build()
            .Run();
    }
}