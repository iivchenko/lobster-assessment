using MediatR;
using System;

namespace Questionnaire.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQuery : IRequest<GetQuestionQueryResponse>
    {
        public Guid PollId { get; set; }

        public Guid QuestionId { get; set; }
    }
}
