using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TodoApp.Api.Http.WebApi.Configuration;
using TodoApp.Api.Http.WebApi.HealthCheck;

namespace TodoApp.Api.Http.WebApi
{
    public static class DependencyInjection
    {
        public static void RegisterWebEntrypointDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            var todoAppConfiguration = new TodoAppConfiguration();
            configuration.Bind(todoAppConfiguration);
            services.AddSingleton(todoAppConfiguration);

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck));

            // TODO: Fix the organization of Endpoints on Swagger view
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TodoApp",
                    Description = "Todo restful API"
                });
            });

            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(todoAppConfiguration.Cors.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}
