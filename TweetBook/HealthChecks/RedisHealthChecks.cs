using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace TweetBook.HealthChecks
{
    public class RedisHealthChecks : IHealthCheck
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisHealthChecks(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                var database = _connectionMultiplexer.GetDatabase();
                database.StringGet("health");
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch(Exception exception)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(exception.Message));
            }
        }
    }
}
