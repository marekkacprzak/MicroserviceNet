using Actio.Common.Commands;
using Actio.Common.Mongo;
using Actio.Common.RabbitMq;
using Actio.Services.Activities.Domain.Repositories;
using Actio.Services.Activities.Handlers;
using Actio.Services.Activities.Repositories;
using Actio.Services.Activities.Services;
using Microsoft.OpenApi.Models;

namespace Actio.Services.Activities;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.DescribeAllParametersInCamelCase();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HTTP Activity API",
                Version = "v1",
                Description = "The Service Activity API"
            });
        });
        services.AddMvc(option => option.EnableEndpointRouting = false);
        services.AddLogging();
        services.AddMongoDB(_configuration);
        services.AddRabbitMq(_configuration);
        services.AddTransient<ICommandHandler<CreateActivity>, CreateActivityHandler>();
        services.AddTransient<IActivityRepository, AtivityRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IDatabaseSeeder, CustomeMongoSeeder>();
        services.AddTransient<IActivityService, ActivityService>();
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
        app.ApplicationServices.GetService<IDatabaseInitializer>()?.InitializeAsync();
        app.UseMvcWithDefaultRoute();
        app.UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTTP Activity API"); });

    }
}