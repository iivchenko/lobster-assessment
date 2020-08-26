﻿using AutoMapper;
using MediatR;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Queries.GetEnd
{
    public sealed class GetEndQueryHandler : IRequestHandler<GetEndQuery, GetEndQueryResponse>
    {
        private readonly IRepository<Poll, Guid> _pollRepository;
        private readonly IMapper _mapper;

        public GetEndQueryHandler(
            IRepository<Poll, Guid> pollRepository,
            IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<GetEndQueryResponse> Handle(GetEndQuery query, CancellationToken cancellationToken)
        {
            var poll = await _pollRepository.Read(query.PollId);

            if (poll == null)
            {
                throw new EntityNotFoundException(query.PollId, nameof(Poll));
            }

            if (!(poll.Items.SingleOrDefault(x => x.Id == query.EndId) is End end))
            {
                throw new EntityNotFoundException(query.EndId, nameof(End));
            }

            return _mapper.Map<GetEndQueryResponse>(end);
        }
    }
}