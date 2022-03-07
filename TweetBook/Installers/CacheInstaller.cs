using TweetBook.Cache;

namespace TweetBook.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            var redisCacheSetting = new RedisCacheSettings();
            builder.Configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSetting);
            builder.Services.AddSingleton(redisCacheSetting);

            if (!redisCacheSetting.Enabled)
            {
                return;
            }

            builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSetting.ConnectionString);
            builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
