using MediatR;
using System;

namespace Story.Application.Queries.GetStory
{
    public sealed class GetStoryQuery : IRequest<GetStoryQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
