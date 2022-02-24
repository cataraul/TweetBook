using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers
{

    public class PostsController :ControllerBase
    {
        private IList<Post> _posts;
        public PostsController()
    public class PostsController :ControllerBase
    {
        private IList<Post> _posts;
        public PostsController()
>>>>>>>>> Temporary merge branch 2
    public class PostsController :ControllerBase
    {
        private IList<Post> _posts;
        public PostsController()
>>>>>>>>> Temporary merge branch 2
    public class PostsController :ControllerBase
    {
        private IList<Post> _posts;
        public PostsController()
>>>>>>>>> Temporary merge branch 2
    public class PostsController : ControllerBase 
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
>>>>>>>>> Temporary merge branch 2
        {
        }
        
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
>>>>>>>>> Temporary merge branch 2

            return Ok(_postService.GetPosts());
        }

>>>>>>>>> Temporary merge branch 2
        public IActionResult Get([FromRoute]Guid postId)
        {

            var post = _postService.GetPostById(postId);
>>>>>>>>> Temporary merge branch 2
<<<<<<<<< Temporary merge branch 1
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);

           
=========
                return NotFound();
>>>>>>>>> Temporary merge branch 2

            return Ok(post);
>>>>>>>>> Temporary merge branch 2
<<<<<<<<< Temporary merge branch 1
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);

           
=========

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)
        {
<<<<<<<<< Temporary merge branch 1
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);

           
=========
            var post = new Post { Id = postRequest.Id };

            if (post.Id == Guid.Empty)
                post.Id = Guid.NewGuid();
<<<<<<<<< Temporary merge branch 1
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);

           
=========
            _postService.GetPosts().Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

<<<<<<<<< Temporary merge branch 1
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);

           
=========
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            return Created(locationUri,post);
        }
    }
}
