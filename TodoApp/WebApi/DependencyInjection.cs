﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TodoApp.WebApi.Configuration;
using TodoApp.WebApi.Controllers.External.v1;
using TodoApp.WebApi.HealthCheck;

namespace TodoApp.WebApi
{
    public static class DependencyInjection
    {
        public static void RegisterWebEntrypointDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddHttpClient<ExternalController>();

            var todoAppConfiguration = new TodoAppConfiguration();
            configuration.Bind(todoAppConfiguration);
            services.AddSingleton(todoAppConfiguration);

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck));

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