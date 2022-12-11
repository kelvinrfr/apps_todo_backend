using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Data.Database;
using TodoApp.Repository;

namespace TodoApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructureDependencyInjection(this IServiceCollection services)
        {
            services.AddDbContext<TodoAppDbContext>();
            services.AddScoped<ITodoRepository, TodoRepository>();

            services.AddMediatR(typeof(DependencyInjection));
        }
    }
}