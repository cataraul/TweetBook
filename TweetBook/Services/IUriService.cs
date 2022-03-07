using Tweetbook.Contracts.V1.Requests.Queries;

namespace TweetBook.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);

        Uri GetAllPostsUri(PaginationQuery paginationQuery = null);
    }
}
