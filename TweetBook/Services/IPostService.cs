using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
        public Task<List<Post>> GetPostsAsync();

       public Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> CreatePostAsync(Post post);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);
    }
}
