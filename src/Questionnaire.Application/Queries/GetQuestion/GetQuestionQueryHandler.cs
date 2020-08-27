using AutoMapper;
using MediatR;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, GetQuestionQueryResponse>
    {
        private readonly IRepository<Poll, Guid> _pollRepository;
        private readonly IMapper _mapper;

        public GetQuestionQueryHandler(
            IRepository<Poll, Guid> pollRepository,
            IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<GetQuestionQueryResponse> Handle(GetQuestionQuery query, CancellationToken cancellationToken)
        {
            var poll = await _pollRepository.Read(query.PollId);

            if (poll == null)
            {
                throw new EntityNotFoundException(query.PollId, nameof(Poll));
            }

            var question = poll.GetQuestion(query.QuestionId);

            if (question == null)
            {
                throw new EntityNotFoundException(query.QuestionId, nameof(Question));
            }

            var answers = poll.FindNextFor(question);

            var response = _mapper.Map<GetQuestionQueryResponse>(question);
            response.Answers = _mapper.Map<IEnumerable<GetQuestionQueryResponseAnswer>>(answers);

            return response;
        }
    }
}
