using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService<Tag, string> _tagsService;
        private readonly IMapper _mapper;

        public TagsController(ITagsService<Tag, string> tagsService,IMapper mapper)
        {
            _tagsService = tagsService;
            _mapper = mapper;
        }
        /// <summary>
        /// Returns all the tags in the system
        /// </summary>
        /// <response code="200">Returns all the tags in the system</response>
        [HttpGet(ApiRoutes.Tags.TagsBase)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagsService.GetAllAsync();
            var tagResponses = tags.Select(tag => new TagResponse { Name = tag.Name }).ToList();

            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        /// <summary>
        /// Creates a tag in the system
        /// </summary>
        /// <response code="201">Creates a tag in the system</response>
        /// <response code="400">Unable to create the tag due to validation error</response>
        [HttpPost(ApiRoutes.Tags.TagsBase)]
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> CreateAsync(CreateTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.TagName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow,
            };

            var created = await _tagsService.CreateAsync(newTag);
            if (!created)
            {
                return BadRequest(error: new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create tag" } } });
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.TagsBase.Replace("{tagName}", newTag.Name);

            return Created(locationUri, _mapper.Map<TagResponse>(newTag));
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
