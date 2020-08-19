using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetStory
{
    public sealed class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, GetStoryQueryResponse>
    {
        public Task<GetStoryQueryResponse> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
