using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System;
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

        public Task<GetStoriesQueryResponse> Handle(GetStoriesQuery request, CancellationToken cancellationToken)
        {
            // Use story repo to get all stories
            // map from domain model to Query model and return
            throw new NotImplementedException();
        }
    }
}
