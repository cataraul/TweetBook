using Swashbuckle.AspNetCore.Filters;
using TweetBook.Contract.V1.Requests;

namespace TweetBook.Examples.Requests
{
    public class CreateTagRequestExample : IExamplesProvider<CreateTagRequest>
    {
        public CreateTagRequest GetExamples()
        {
            return new CreateTagRequest
            {
                TagName = "new tag"
            };
        }
    }
}
