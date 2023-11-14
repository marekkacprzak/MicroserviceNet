using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.Services;
using Actio.Common.Servoces;

namespace Actio.Services.Identity;

public class Program
{
    public static async Task Main(string[] args)
    {
        await ServiceHost.Create<Startup>(args)
            .UseRabbitMq()
            .SubscribeToCommand<CreateUser>()
            .Build()
            .Run();
    }
}