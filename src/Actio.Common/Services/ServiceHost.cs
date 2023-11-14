using System.Net.Http.Headers;
using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.RabbitMq;
using Actio.Common.Servoces;
using Microsoft.AspNetCore;
using RawRabbit;

namespace Actio.Common.Services;

public class ServiceHost : IServiceHost
{
    private readonly IWebHost _webHost;

    public ServiceHost(IWebHost webHost)
    {
        _webHost = webHost;
    }

    public async Task Run() => await _webHost.RunAsync();

    public static HostBuilder Create<TStartup>(string[] ard) 
        where TStartup : class
    {
        Console.Title = typeof(TStartup).Namespace!;
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(ard)
            .Build();
        var webHostBuilder = WebHost.CreateDefaultBuilder(ard)
            .UseConfiguration(config)
            .UseStartup<TStartup>();
        return new HostBuilder(webHostBuilder.Build());
    }

    public abstract class BuilderBase
    {
        public abstract ServiceHost Build();
    }

    public class HostBuilder(IWebHost? webHost) : BuilderBase
    {
        private IBusClient? _bus;

        public HostBuilder():this(null)
        {
        }

        public BasBuilder UseRabbitMq()
        {
            var client=webHost?.Services.GetService(typeof(IBusClient)) as IBusClient;
            if (client!=null)
                _bus = client;
             if (_bus is null)
                    throw new Exception("Services.GetService(typeof(IBusClient)) as IBusClient");
               return new BasBuilder(webHost!, _bus);
        }
        public override ServiceHost Build()
        {
            return new ServiceHost(webHost!);
        }
    }

    public class BasBuilder:BuilderBase
    {
        private readonly IWebHost _webHost;
        private readonly IBusClient _busClient;

        public BasBuilder(IWebHost webHost, IBusClient busClient)
        {
            _webHost = webHost;
            _busClient = busClient;
        }

        public BasBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
        {
            // var hendler = (ICommandHandler<TCommand>)_webHost.Services.GetService(typeof(ICommandHandler<TCommand>));
            // _busClient.WithCommandHandlerAsync(hendler);
            // return this;
            
            using (var serviceScope = _webHost.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var handler = (ICommandHandler<TCommand>)serviceScope.ServiceProvider
                    .GetService(typeof(ICommandHandler<TCommand>))!;
                _busClient.WithCommandHandlerAsync(handler);
            }
            return this;
        }
        public BasBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
        {
            using (var serviceScope = _webHost.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var handler =
                    serviceScope.ServiceProvider.GetService(typeof(IEventHandler<TEvent>)) as IEventHandler<TEvent>;
                if (handler is null)
                    throw new Exception("ServiceProvider.GetService(typeof(IEventHandler<TEvent>)) as IEventHandler<TEvent>");
                _busClient.WithEventHandlerAsync(handler);
            }
            return this;
        }
        public override ServiceHost Build()
        {
            return new ServiceHost(_webHost);
        }
    }
}

