using AutoMapper;
using Story.Application.Domain.Polls;
using Story.Application.Queries.GetAnswer;
using Story.Application.Queries.GetEnd;
using Story.Application.Queries.GetPoll;
using Story.Application.Queries.GetPolls;
using Story.Application.Queries.GetQuestion;
using Story.Host.Polls;

namespace Story.Host
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Polls Flow
            CreateMap<Poll, GetPollsQueryPollSummary>();
            CreateMap<GetPollsQueryPollSummary, PollSummaryViewModel>();

            // Single poll flow
            CreateMap<Poll, GetPollQueryResponse>()
                .ForMember(dest => dest.RootQuestionId, opt => opt.MapFrom(src => src.RootQuestion.Id));
            CreateMap<GetPollQueryResponse, PollViewModel>();

            // Question flow
            CreateMap<Question, GetQuestionQueryResponse>();
            CreateMap<Answer, GetQuestionQueryResponseAnswer>();
            CreateMap<GetQuestionQueryResponse, QuestionViewModel>();
            CreateMap<GetQuestionQueryResponseAnswer, AnswerSummaryViewModel>();

            // Answer flow
            CreateMap<Answer, GetAnswerQueryResponse>();
            CreateMap<PollItem, GetAnswerQueryResponseNext>()
               .Include<Question, GetAnswerQueryResponseNextQuestion>()
               .Include<End, GetAnswerQueryResponseNextEnd>();
            CreateMap<Question, GetAnswerQueryResponseNextQuestion>();
            CreateMap<End, GetAnswerQueryResponseNextEnd>();
            CreateMap<GetAnswerQueryResponse, AnswerViewModel>()
                .ForMember(dest => dest.NextEntityType, opt => opt.MapFrom(src => src.Next is GetAnswerQueryResponseNextQuestion ? NextEntityType.Question : NextEntityType.End))
                .ForMember(dest => dest.NextEntityId, opt => opt.MapFrom(src => src.Next.Id));

            // End flow
            CreateMap<End, GetEndQueryResponse>();
            CreateMap<GetEndQueryResponse, EndViewModel>();
        }
    }
}
