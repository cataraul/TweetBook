using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract;
using TweetBook.Contract.V1.Requests;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService<Tag, string> _tagsService;

        public TagsController(ITagsService<Tag, string> tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tagsService.GetAllAsync());
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> CreateAsync(CreateTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.tagName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow,
            };

            var created = await _tagsService.CreateAsync(newTag);
            if (!created)
            {
                return BadRequest(error: new { error = "Unable to create tag" });
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);
            return Created(locationUri, newTag);
        }
        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Policy = "MustWorkForRaul")]
        public async Task<IActionResult> Delete([FromRoute]string tagName)
        {
            var deleted = await _tagsService.DeleteAsync(tagName);

            if (deleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
