using AutoMapper;
using Story.Application.Domain.Polls;
using Story.Application.Domain.Stories;
using Story.Application.Domain.Stories.Abstractions;
using Story.Application.Queries.GetAnswer;
using Story.Application.Queries.GetEnd;
using Story.Application.Queries.GetFullStory;
using Story.Application.Queries.GetPoll;
using Story.Application.Queries.GetPolls;
using Story.Application.Queries.GetQuestion;
using Story.Host.Polls;
using Story.Host.Stories;
using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Host
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Poll, GetPollsQueryPollSummary>();
            CreateMap<GetPollsQueryPollSummary, PollSummaryViewModel>();

            CreateMap<Poll, GetPollQueryResponse>()
                .ForMember(dest => dest.RootQuestionId, opt => opt.MapFrom(src => src.RootQuestion.Id));
            CreateMap<GetPollQueryResponse, PollViewModel>();

            CreateMap<PollQuestion, GetQuestionQueryResponse>();
            CreateMap<PollAnswer, GetQuestionQueryResponseAnswer>();
            CreateMap<GetQuestionQueryResponse, QuestionViewModel>();
            CreateMap<GetQuestionQueryResponseAnswer, AnswerSummaryViewModel>();

            CreateMap<PollAnswer, GetAnswerQueryResponse>();
            CreateMap<PollItem, GetAnswerQueryResponseNext>()
               .Include<PollQuestion, GetAnswerQueryResponseNextQuestion>()
               .Include<PollEnd, GetAnswerQueryResponseNextEnd>();
            CreateMap<PollQuestion, GetAnswerQueryResponseNextQuestion>();
            CreateMap<PollEnd, GetAnswerQueryResponseNextEnd>();
            CreateMap<GetAnswerQueryResponse, AnswerViewModel>()
                .ForMember(dest => dest.NextEntityType, opt => opt.MapFrom(src => src.Next is GetAnswerQueryResponseNextQuestion ? NextEntityType.Question : NextEntityType.End))
                .ForMember(dest => dest.NextEntityId, opt => opt.MapFrom(src => src.Next.Id));

            CreateMap<PollEnd, GetEndQueryResponse>();
            CreateMap<GetEndQueryResponse, EndViewModel>();

            CreateMap<AStory, GetFullStoryQueryResponse>();
            CreateMap<Question, GetFullStoryQueryResponseQuestion>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Nodes));
            CreateMap<TheEnd, GetFullStoryQueryResponseEnd>();
            CreateMap<Answer, GetFullStoryQueryResponseAnswer>()
                .ForMember(dest => dest.NextType, opt => opt.MapFrom(src => src.Next is Question ? GetFullStoryQueryResponseAnswerNextType.Question : GetFullStoryQueryResponseAnswerNextType.End));
            CreateMap<NodeLeaf, GetFullStoryQueryResponseAnswerNext>()
                .Include<Question, GetFullStoryQueryResponseQuestion>()
                .Include<TheEnd, GetFullStoryQueryResponseEnd>();

            CreateMap<GetFullStoryQueryResponse, FullStoryViewModel>();
            CreateMap<GetFullStoryQueryResponseQuestion, FullQuestionViewModel>();
            CreateMap<GetFullStoryQueryResponseEnd, FullEndViewModel>();
            CreateMap<GetFullStoryQueryResponseAnswer, FullAnswerViewModel>();
            CreateMap<GetFullStoryQueryResponseAnswerNext, FullAnswerNextViewModel>()
               .Include<GetFullStoryQueryResponseQuestion, FullQuestionViewModel>()
               .Include<GetFullStoryQueryResponseEnd, FullEndViewModel>();
        }
    }
}
