using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Infrastructure.HealthCheck;

namespace TodoApp.Api.Http.HealthCheck
{
	public class DatabaseHealthCheck : IHealthCheck
	{

		private readonly IMediator _mediator;

		public DatabaseHealthCheck(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
		{
			var request = new DababaseHealthCheckRequest();

			DatabaseState result = await _mediator.Send(request, cancellationToken);

			if (result == DatabaseState.Running)
			{
				return HealthCheckResult.Healthy();
			}

			return HealthCheckResult.Unhealthy();
		}
	}
}