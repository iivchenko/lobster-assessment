﻿using AutoMapper;
using MediatR;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQueryHandler : IRequestHandler<GetAnswerQuery, GetAnswerQueryResponse>
    {
        private readonly IRepository<Poll, Guid> _pollRepository;
        private readonly IMapper _mapper;

        public GetAnswerQueryHandler(
            IRepository<Poll, Guid> pollRepository,
            IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<GetAnswerQueryResponse> Handle(GetAnswerQuery query, CancellationToken cancellationToken)
        {
            var poll = await _pollRepository.Read(query.PollId);

            if (poll == null)
            {
                throw new EntityNotFoundException(query.PollId, nameof(Poll));
            }

            var answer = poll.GetAnswer(query.AnswerId);

            if (answer == null)
            {
                throw new EntityNotFoundException(query.AnswerId, nameof(Answer));
            }

            var next = poll.FindNextFor(answer);

            var response = _mapper.Map<GetAnswerQueryResponse>(answer);
            response.Next = _mapper.Map<GetAnswerQueryResponseNext>(next);

            return response;
        }
    }
}
