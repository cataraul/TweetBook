using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    { 
       public Task<IList<Post>> GetAllAsync(GetAllPostsFilter? filter = null, PaginationFilter? paginationFilter = null);

       public Task<Post> GetPostByIdAsync(Guid postId);

       public Task<bool> CreatePostAsync(Post post);

       public Task<bool> UpdatePostAsync(Post postToUpdate);

       public Task<bool> DeletePostAsync(Guid postId);

       public Task<bool> UserOwnsPostAsync(Guid postId, string getUserId);
    }
}
