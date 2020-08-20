using AutoMapper;
using Story.Application.Queries.GetStories;
using Story.Host.Stories;
using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Host
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AStory, GetStoryQueryStorySummary>();
            CreateMap<GetStoryQueryStorySummary, StorySummaryViewModel>();
        }
    }
}
