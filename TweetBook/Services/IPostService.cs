using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
       public List<Post> GetPosts();

       public Post GetPostById(Guid postId);

       public bool UpdatePost(Post postToUpdate);

       public bool DeletePost(Guid postId);
    }
}
