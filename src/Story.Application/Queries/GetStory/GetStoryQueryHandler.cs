using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
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

        public async Task<GetStoryQueryResponse> Handle(GetStoryQuery query, CancellationToken cancellationToken)
        {
            var story = await _storyRepository.Read(query.Id);

            if (story == null)
            {
                throw new EntityNotFoundException(query.Id);
            }

            return _mapper.Map<GetStoryQueryResponse>(story);
        }
    }
}
