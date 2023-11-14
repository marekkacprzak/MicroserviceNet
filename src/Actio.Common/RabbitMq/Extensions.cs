using System.Reflection;
using Actio.Common.Commands;
using Actio.Common.Events;
using RawRabbit;
using RawRabbit.Instantiation;
using RawRabbit.Pipe.Middleware;

namespace Actio.Common.RabbitMq;

public static class Extensions
{
    public static Task WithCommandHandlerAsync<TCommand>(this IBusClient busClient,
        ICommandHandler<TCommand> commandHandler)
        where TCommand : ICommand =>
        busClient.SubscribeAsync<TCommand>(msg => commandHandler.HandleAsync(msg),
            ctx => ctx.UseConsumeConfiguration(cfg =>
                cfg.FromQueue(GetQueueName<TCommand>())));

    public static Task WithEventHandlerAsync<TEvent>(this IBusClient busClient,
        IEventHandler<TEvent> commandHandler)
        where TEvent : IEvent =>
        busClient.SubscribeAsync<TEvent>(msg => commandHandler.HandleAsync(msg),
            ctx => ctx.UseConsumeConfiguration(cfg =>
                cfg.FromQueue(GetQueueName<TEvent>())));

    private static string GetQueueName<T>() => $"{Assembly.GetEntryAssembly()!.GetName()}/{typeof(T).Name}";

    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new RabbitMqOption();
        var section = configuration.GetSection("rabbitmq");
        section.Bind(options);
        var client = RawRabbitFactory.CreateSingleton(
            new RawRabbitOptions
            {
                ClientConfiguration = options
            });
        services.AddSingleton<IBusClient>(_ => client);
    }

}