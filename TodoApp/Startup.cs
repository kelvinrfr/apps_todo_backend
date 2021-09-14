using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TodoApp.Configuration;
using TodoApp.Data.Database;
using TodoApp.Service;
using TodoApp.Repository;

namespace TodoApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var todoAppConfiguration = new TodoAppConfiguration();
            Configuration.Bind(todoAppConfiguration);
            services.AddSingleton(todoAppConfiguration);

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

            services.AddControllers();
            services.AddEntityFrameworkNpgsql().AddDbContext<TodoAppDbContext>();

            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<ITodoRepository, TodoRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp v1"));
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
