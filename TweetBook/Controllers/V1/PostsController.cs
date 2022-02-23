﻿using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract;
using TweetBook.Domain;

namespace TweetBook.Controllers
{
    public class PostsController :ControllerBase
    {
        private List<Post> _posts;
        public PostsController()
        {
            _posts = new List<Post>();
            for(int i = 0; i < 5; i++)
            {
                _posts.Add(new Post { Id = Guid.NewGuid().ToString() });
            }
        }
        
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }
    }
}
