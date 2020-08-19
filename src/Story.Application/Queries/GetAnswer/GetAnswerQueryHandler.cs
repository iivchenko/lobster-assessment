using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQueryHandler : IRequestHandler<GetAnswerQuery, GetAnswerQueryResponse>
    {
        public Task<GetAnswerQueryResponse> Handle(GetAnswerQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
