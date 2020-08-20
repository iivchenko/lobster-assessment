using MediatR;
using System;

namespace Story.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQuery : IRequest<GetQuestionQueryResponse>
    {
        public Guid StoryId { get; set; }

        public Guid QuestionId { get; set; }
    }
}
