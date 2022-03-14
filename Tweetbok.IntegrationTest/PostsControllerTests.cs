using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TweetBook.Contract;
using TweetBook.Contract.V1.Requests;
using TweetBook.Domain;
using Xunit;

namespace Tweetbok.IntegrationTests
{
    public class PostsControllerTests : IntegrationTests
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.Empty(await response.Content.ReadAsAsync<List<Post>>());
        }

        [Fact]
        public async Task Get_ReturnsPostWhenPostExistInTheDatabase()
        {
            //Arrange
            var auth = await AuthenticateAsync();
            var createdPost = await CreatePostAsync(new CreatePostRequest { Name = "test post", Tags = new string[] { "test tag" }});

            //Act
            var responseGetAll = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);
            var response = await TestClient.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", createdPost.Id.ToString()));
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = await response.Content.ReadAsAsync<Post>();
            returnedPost.Id.Should().Be(createdPost.Id);
            returnedPost.Name.Should().Be("Test Post");
        }
    }
}
