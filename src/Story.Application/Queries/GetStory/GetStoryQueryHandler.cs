using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetStory
{
    public sealed class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, GetStoryQueryResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;

        public GetStoryQueryHandler(
            IStoryRepository storyRepository,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        public Task<GetStoryQueryResponse> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            // Get story by Id
            // map and rturn
            throw new NotImplementedException();
        }
    }
}
