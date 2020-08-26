using MediatR;
using System;

namespace Questionnaire.Application.Queries.GetPoll
{
    public sealed class GetPollQuery : IRequest<GetPollQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
