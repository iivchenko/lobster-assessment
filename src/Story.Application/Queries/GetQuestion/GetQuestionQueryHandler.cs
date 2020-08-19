using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, GetQuestionResponse>
    {
        public Task<GetQuestionResponse> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
