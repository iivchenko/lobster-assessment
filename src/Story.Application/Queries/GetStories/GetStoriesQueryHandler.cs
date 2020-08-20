using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetStories
{
    public sealed class GetStoriesQueryHandler : IRequestHandler<GetStoriesQuery, GetStoriesQueryResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;

        public GetStoriesQueryHandler(
            IStoryRepository storyRepository,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        public async Task<GetStoriesQueryResponse> Handle(GetStoriesQuery request, CancellationToken cancellationToken)
        {
            var stories = await _storyRepository.ReadAll();

            return new GetStoriesQueryResponse
            {
                Stories = _mapper.Map<IEnumerable<GetStoryQueryStorySummary>>(stories)
            };
        }
    }
}
