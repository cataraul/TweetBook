using TweetBook.HealthChecks;

namespace TweetBook.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        void IInstaller.InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<DataContext>()
                .AddCheck<RedisHealthChecks>("Redis");
        }
    }
}
