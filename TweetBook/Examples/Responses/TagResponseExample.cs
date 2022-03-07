using Swashbuckle.AspNetCore.Filters;
using TweetBook.Contract.V1.Responses;

namespace TweetBook.Examples.Responses
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "new tag",
            };
        }
    }
}
