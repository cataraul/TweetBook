using Refit;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;

namespace Tweetbook.Sdk
{
    public interface IIdentityApi
    {
        [Post("/api/v1/identity/register")]
        Task<ApiResponse<AuthSuccesResponse>> RegisterAsync([Body] UserRegistrationRequest registrationRequest);

        [Post("/api/v1/identity/login")]
        Task<ApiResponse<AuthSuccesResponse>> LoginAsync([Body] UserLoginRequest loginRequest);

        [Post("/api/v1/identity/refresh")]
        Task<ApiResponse<AuthSuccesResponse>> RefreshAsync([Body] RefreshTokenRequest refreshRequest);
    }
}
