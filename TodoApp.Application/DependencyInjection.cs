using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Application
{
    public static class DependencyInjection
    {
        public static void RegisterApplicationDependencyInjection(this IServiceCollection services)
        {
            // TODO: how to remove these framework dependencies from Application layer?
            services.AddMediatR(typeof(DependencyInjection));
            services.AddAutoMapper(typeof(DependencyInjection));
        }
    }
}