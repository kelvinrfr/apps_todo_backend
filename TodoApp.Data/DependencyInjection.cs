using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Services.Repositories;
using TodoApp.Data.Database;
using TodoApp.Domain.Todo;
using TodoApp.Infrastructure.Database.Repository;

namespace TodoApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructureDependencyInjection(this IServiceCollection services)
        {
            services.AddDbContext<TodoAppDbContext>();
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();

            services.AddMediatR(typeof(DependencyInjection));
        }
    }
}