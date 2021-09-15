using TodoApp.Configuration;
using TodoApp.Data.Database;
using TodoApp.Service;
using TodoApp.Repository;
using Microsoft.OpenApi.Models;
using TodoApp.HealthCheck;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<ITodoService, TodoService>();
        }

        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck));
        }
        
        public static void ConfigureCors(this IServiceCollection services, TodoAppConfiguration configuration)
        {
            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(configuration.Cors.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TodoApp",
                    Description = "Todo restful API"
                });
            });
        }

        public static void ConfigureDataProviders(this IServiceCollection services)
        {
            services.AddDbContext<TodoAppDbContext>();
            services.AddScoped<ITodoRepository, TodoRepository>();
        }
    }
}