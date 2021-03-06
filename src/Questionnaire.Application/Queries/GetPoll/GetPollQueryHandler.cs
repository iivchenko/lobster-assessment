﻿using AutoMapper;
using MediatR;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Queries.GetPoll
{
    public sealed class GetPollQueryHandler : IRequestHandler<GetPollQuery, GetPollQueryResponse>
    {
        private readonly IRepository<Poll, Guid> _pollRepository;
        private readonly IMapper _mapper;

        public GetPollQueryHandler(
            IRepository<Poll, Guid> pollRepository,
            IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<GetPollQueryResponse> Handle(GetPollQuery query, CancellationToken cancellationToken)
        {
            var poll = await _pollRepository.Read(query.Id);

            if (poll == null)
            {
                throw new EntityNotFoundException(query.Id, nameof(Poll));
            }

            return _mapper.Map<GetPollQueryResponse>(poll);
        }
    }
}
