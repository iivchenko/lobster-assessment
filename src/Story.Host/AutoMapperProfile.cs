﻿using AutoMapper;
using Story.Application.Domain.Stories;
using Story.Application.Queries.GetAnswer;
using Story.Application.Queries.GetEnd;
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

            CreateMap<Answer, GetAnswerQueryResponse>();
            CreateMap<Question, GetAnswerQueryResponseNext>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetAnswerQueryResponseNextType.Question));
            CreateMap<TheEnd, GetAnswerQueryResponseNext>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetAnswerQueryResponseNextType.End));
            CreateMap<GetAnswerQueryResponse, AnswerViewModel>()
                .ForMember(dest => dest.NextEntityType, opt => opt.MapFrom(src => src.Next.Type))
                .ForMember(dest => dest.NextEntityId, opt => opt.MapFrom(src => src.Next.Id));

            CreateMap<TheEnd, GetEndQueryResponse>();
            CreateMap<GetEndQueryResponse, EndViewModel>();
        }
    }
}
