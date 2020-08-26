using MediatR;
using System;

namespace Story.Application.Queries.GetPoll
{
    public sealed class GetPollQuery : IRequest<GetPollQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
