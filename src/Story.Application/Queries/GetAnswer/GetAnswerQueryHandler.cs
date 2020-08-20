using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQueryHandler : IRequestHandler<GetAnswerQuery, GetAnswerQueryResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;

        public GetAnswerQueryHandler(
            IStoryRepository storyRepository,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        public Task<GetAnswerQueryResponse> Handle(GetAnswerQuery request, CancellationToken cancellationToken)
        {
            // get story
            // if no - throws
            // find - answer
            // if no - throws
            throw new NotImplementedException();
        }
    }
}
