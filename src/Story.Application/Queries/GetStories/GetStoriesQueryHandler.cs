using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetStories
{
    public sealed class GetStoriesQueryHandler : IRequestHandler<GetStoriesQuery, GetStoriesQueryResponse>
    {
        public Task<GetStoriesQueryResponse> Handle(GetStoriesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
