using System.Diagnostics;
using Actio.Api.Handlers;
using Actio.Api.Repositories;
using Actio.Common.Auth;
using Actio.Common.Events;
using Actio.Common.Mongo;
using Actio.Common.RabbitMq;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Actio.Api;

public class Startup(IConfiguration _configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.DescribeAllParametersInCamelCase();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HTTP API",
                Version = "v1",
                Description = "The Service HTTP API"
            });
        });
        services.AddMvc(option => option.EnableEndpointRouting = false);
        services.AddLogging();
        services.AddMongoDB(_configuration);
        services.AddJwt(_configuration);
        services.AddRabbitMq(_configuration);
        services.AddTransient<IEventHandler<ActivityCreated>, ActivityCreatedHandler>();
        services.AddTransient<IEventHandler<UserAuthenticated>, UserAuthenticatedHandler>();
        services.AddTransient<IEventHandler<UserCreated>, UserCreatedHandler>();
        services.AddTransient<IActivityRepository, ActivityRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseCors();

        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
        // });
        app.UseMvcWithDefaultRoute();
        app.UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTTP API V1"); });
    }
}