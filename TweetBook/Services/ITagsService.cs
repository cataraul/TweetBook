using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface ITagsService<TItem,TKey>
    {
       public Task<IEnumerable<TItem>> GetAllAsync();

       public Task<TItem> GetAsync(string requestName);

       public Task<bool> CreateAsync(TItem item);

       public Task<bool> DeleteAsync(TKey key);
    }
}
