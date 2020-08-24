using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetFullStory
{
    public sealed class GetFullStoryQueryHandler : IRequestHandler<GetFullStoryQuery, GetFullStoryQueryResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;

        public GetFullStoryQueryHandler(
            IStoryRepository storyRepository,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        public async Task<GetFullStoryQueryResponse> Handle(GetFullStoryQuery query, CancellationToken cancellationToken)
        {
            var story = await _storyRepository.Read(query.Id);

            if (story == null)
            {
                throw new EntityNotFoundException(query.Id, nameof(Story));
            }

            return _mapper.Map<GetFullStoryQueryResponse>(story);
        }
    }
}
