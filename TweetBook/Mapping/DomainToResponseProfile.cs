using AutoMapper;
using TweetBook.Contract.V1.Responses;
using TweetBook.Domain;

namespace TweetBook.Mapping
{
    public class DomainToResponseProfile : Profile
    {
      public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(destination=>destination.Tags,options=>
                options.MapFrom(source=>source.Tags.Select(tagResponse=>new TagResponse { Name=tagResponse.TagName})));

            CreateMap<Tag, TagResponse>();
        }
    }
}
