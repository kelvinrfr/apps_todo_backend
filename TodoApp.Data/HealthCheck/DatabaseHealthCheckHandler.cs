using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.HealthCheck
{
    public class DababaseHealthCheckRequest : IRequest<DatabaseState> { }

    public enum DatabaseState { Running, Failing }

    internal class DatabaseHealthCheckHandler : IRequestHandler<DababaseHealthCheckRequest, DatabaseState>
    {
        private const string _cacheKey = "database_check";
        private readonly IMemoryCache _memoryCache;

        public DatabaseHealthCheckHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<DatabaseState> Handle(DababaseHealthCheckRequest request, CancellationToken cancellationToken)
        {
            if(_memoryCache.TryGetValue(_cacheKey, out var result)) 
            {
                if(Enum.TryParse(result.ToString(), out DatabaseState dbResult))
                {
                    return Task.FromResult(dbResult);
                }
            }

            var ttl = DateTimeOffset.Now.AddSeconds(10);

            DatabaseState dbCurrentState = DatabaseState.Running;

            _memoryCache.Set(_cacheKey, dbCurrentState, ttl);

            return Task.FromResult(dbCurrentState);
        }
    }
}