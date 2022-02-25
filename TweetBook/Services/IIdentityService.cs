using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IIdentityService
    {
      public  Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}
