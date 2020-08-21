using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using Story.Application.Domain.Stories.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetEnd
{
    public sealed class GetEndQueryHandler : IRequestHandler<GetEndQuery, GetEndQueryResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;

        public GetEndQueryHandler(
            IStoryRepository storyRepository,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        public async Task<GetEndQueryResponse> Handle(GetEndQuery query, CancellationToken cancellationToken)
        {
            var story = await _storyRepository.Read(query.StoryId);

            if (story == null)
            {
                throw new EntityNotFoundException(query.StoryId, nameof(Story));
            }

            var end = Find(story.Root, query.EndId);

            if (end == null)
            {
                throw new EntityNotFoundException(query.EndId, nameof(TheEnd));
            }

            return _mapper.Map<GetEndQueryResponse>(end);
        }

        private static TheEnd Find(NodeLeaf node, Guid id)
        {
            switch(node)
            {
                case TheEnd end when end.Id == id:
                    return end;

                case Answer answer:
                    return Find(answer.Next, id);

                case NodeTree tree:
                    foreach(var n in tree.Nodes)
                    {
                        var result = Find(n, id);

                        if (result != null)
                        {
                            return result;
                        }
                    }

                    return null;

                default:
                    return null;
            }
        }
    }
}
