using AutoMapper;
using Story.Application.Domain.Stories;
using Story.Application.Queries.GetQuestion;
using Story.Application.Queries.GetStories;
using Story.Application.Queries.GetStory;
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

            CreateMap<AStory, GetStoryQueryResponse>()
                .ForMember(dest => dest.RootQuestionId, opt => opt.MapFrom(src => src.Root.Id));
            CreateMap<GetStoryQueryResponse, StoryViewModel>();

            CreateMap<Question, GetQuestionQueryResponse>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Nodes));
            CreateMap<Answer, GetQuestionQueryResponseAnswer>();
            CreateMap<GetQuestionQueryResponse, QuestionViewModel>();
            CreateMap<GetQuestionQueryResponseAnswer, AnswerSummaryViewModel>();
        }
    }
}
