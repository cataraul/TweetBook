using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TweetBook.Contract;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;
using TweetBook.Data;

namespace Tweetbok.IntegrationTests
{
    public class IntegrationTests : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider serviceProvider;

        protected IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<DataContext>));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase(databaseName: "TestDb");
                        });
                    });
                });
            serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        protected async Task<AuthenticationHeaderValue> AuthenticateAsync()
        {
           return TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Posts.Create, request);

            return await response.Content.ReadAsAsync<PostResponse>();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "test@rauls.com",
                Password = "SomePass1234!"
            });

            //var registrationResponse = await response.Content.ReadAsAsync<AuthSuccesResponse>();
            //return registrationResponse.Token;
            var registrationResponse = await response.Content.ReadAsStringAsync();
            AuthSuccesResponse resp = JsonConvert.DeserializeObject<AuthSuccesResponse>(registrationResponse);
            return resp.Token;
        }

        public void Dispose()
        {
            using var serviceScope = serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureDeleted();
        }
    }
}
