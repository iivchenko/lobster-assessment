using AutoMapper;
using Questionnaire.Application.Domain.Polls;
using Questionnaire.Application.Queries.GetAnswer;
using Questionnaire.Application.Queries.GetEnd;
using Questionnaire.Application.Queries.GetPoll;
using Questionnaire.Application.Queries.GetPolls;
using Questionnaire.Application.Queries.GetQuestion;
using Questionnaire.Host.Polls;

namespace Questionnaire.Host
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
                .ForMember(dest => dest.NextEntityType, opt => opt.MapFrom(src => src.Next is GetAnswerQueryResponseNextQuestion ? AnswerViewModel.EntityType.Question : AnswerViewModel.EntityType.End))
                .ForMember(dest => dest.NextEntityId, opt => opt.MapFrom(src => src.Next.Id));

            // End flow
            CreateMap<End, GetEndQueryResponse>();
            CreateMap<GetEndQueryResponse, EndViewModel>();
        }
    }
}
