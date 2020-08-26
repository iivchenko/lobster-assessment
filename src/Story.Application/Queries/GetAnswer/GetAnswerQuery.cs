using MediatR;
using System;

namespace Story.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQuery : IRequest<GetAnswerQueryResponse>
    {
        public Guid PollId { get; set; }

        public Guid AnswerId { get; set; }
    }
}
