﻿using Refit;
using Tweetbook.Sdk;
using TweetBook.Contract.V1.Requests;

var cachedToken = string.Empty;

var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
var tweetbookApi = RestService.For<ITweetbookApi>("https://localhost:5001",new RefitSettings
    {
        AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
    });

var registerResponse = await  identityApi.RegisterAsync(new UserRegistrationRequest
    {
        Email = "sdkaccount@gmail.com",
        Password = "Test1234!"
    });
var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
    {
        Email = "sdkaccount@gmail.com",
        Password = "Test1234!"
    });

cachedToken = loginResponse.Content.Token;

var allPosts = await tweetbookApi.GetAllAsync();

var createdPost = await tweetbookApi.CreateAsync(new CreatePostRequest
{
    Name = "This is created by the SDkK",
    Tags = new[] { "skd1", "skd2" }
});

var retrievedPost = await tweetbookApi.GetAsync(createdPost.Content.Id);

var updatedPost = await tweetbookApi.UpdateAsync(createdPost.Content.Id, new UpdatePostRequest
{
    Name = "This is updated by the SDK"
});

var deletePost = await tweetbookApi.DeleteAsync(createdPost.Content.Id);
