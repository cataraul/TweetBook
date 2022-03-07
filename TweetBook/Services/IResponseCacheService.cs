namespace TweetBook.Services
{
    public interface IResponseCacheService
    {
        public Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive);

        public Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
