using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
       public List<Post> GetPosts();

       public Post GetPostById(Guid postId);

        bool UpdatePost(Post postToUpdate);
    }
}
