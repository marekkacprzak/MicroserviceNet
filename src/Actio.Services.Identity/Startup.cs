using Actio.Common.Auth;
using Actio.Common.Commands;
using Actio.Common.Mongo;
using Actio.Common.RabbitMq;
using Actio.Services.Identity.Domain.Repositories;
using Actio.Services.Identity.Domain.Services;
using Actio.Services.Identity.Handlers;
using Actio.Services.Identity.Repositories;
using Actio.Services.Identity.Services;
using Microsoft.OpenApi.Models;

namespace Actio.Services.Identity;

public class Startup
{
    private readonly IConfiguration _configuration;
    //private readonly IOptions<JwtOptions> _options;

    public Startup(IConfiguration configuration)
        //IOptions<JwtOptions> options)
    {
        _configuration = configuration;
     //   _options = options;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.DescribeAllParametersInCamelCase();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HTTP Identity Api",
                Version = "v1",
                Description = "The Service HTTP Identity API"
            });
        });
        services.AddMvc(option => option.EnableEndpointRouting = false);
        services.AddLogging();
        services.AddJwt(_configuration);
        services.AddMongoDB(_configuration);
        services.AddRabbitMq(_configuration);
        services.AddTransient<ICommandHandler<CreateUser>, CreateUserHandler>();
        services.AddTransient<IEncrypter, Encrypter>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserService, UserService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseCors();
        app.UseMvcWithDefaultRoute();
        app.UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTTP Identity Api"); });
        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
        // });

    }
}