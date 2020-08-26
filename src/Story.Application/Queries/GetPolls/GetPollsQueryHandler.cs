using AutoMapper;
using MediatR;
using Story.Application.Domain.Common;
using Story.Application.Domain.Polls;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetPolls
{
    public sealed class GetPollsQueryHandler : IRequestHandler<GetPollsQuery, GetPollsQueryResponse>
    {
        private readonly IRepository<Poll, Guid> _pollRepository;
        private readonly IMapper _mapper;

        public GetPollsQueryHandler(
            IRepository<Poll, Guid> pollRepository,
            IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<GetPollsQueryResponse> Handle(GetPollsQuery request, CancellationToken cancellationToken)
        {
            var count = await _pollRepository.ReadCount();
            var polls = await _pollRepository.ReadAll(0, count);

            return new GetPollsQueryResponse
            {
                Polls = _mapper.Map<IEnumerable<GetPollsQueryPollSummary>>(polls)
            };
        }
    }
}
