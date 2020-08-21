using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<GetAnswerQueryResponse> Handle(GetAnswerQuery query, CancellationToken cancellationToken)
        {
            var story = await _storyRepository.Read(query.StoryId);

            if (story == null)
            {
                throw new EntityNotFoundException(query.StoryId, nameof(Story));
            }

            var answer = Find(story.Root.Nodes.Cast<Answer>(), query.AnswerId);

            if (answer == null)
            {
                throw new EntityNotFoundException(query.AnswerId, nameof(Answer));
            }

            return _mapper.Map<GetAnswerQueryResponse>(answer);
        }

        private static Answer Find(IEnumerable<Answer> answers, Guid id)
        {
            foreach(var answer in answers)
            {
                if (answer.Id == id)
                {
                    return answer;
                }

                var nested = (answer.Next as Question)?.Nodes.Cast<Answer>() ?? Enumerable.Empty<Answer>();

                var item = Find(nested, id);

                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
