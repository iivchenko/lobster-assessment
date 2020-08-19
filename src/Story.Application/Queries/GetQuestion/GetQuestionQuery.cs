using MediatR;
using System;

namespace Story.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQuery : IRequest<GetQuestionResponse>
    {
        public Guid StoryId { get; set; }

        public Guid QuestionId { get; set; }
    }
}
