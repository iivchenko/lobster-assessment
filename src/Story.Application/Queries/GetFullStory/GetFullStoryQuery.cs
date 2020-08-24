using MediatR;
using System;

namespace Story.Application.Queries.GetFullStory
{
    public sealed class GetFullStoryQuery : IRequest<GetFullStoryQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
