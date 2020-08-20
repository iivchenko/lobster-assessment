using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, GetQuestionQueryResponse>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;

        public GetQuestionQueryHandler(
            IStoryRepository storyRepository,
            IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        public async Task<GetQuestionQueryResponse> Handle(GetQuestionQuery query, CancellationToken cancellationToken)
        {
            var story = await _storyRepository.Read(query.StoryId);

            if (story == null)
            {
                throw new EntityNotFoundException(query.StoryId, nameof(Story));
            }

            var question = Find(story.Root, query.QuestionId);

            if (question == null)
            {
                throw new EntityNotFoundException(query.QuestionId, nameof(Question));
            }

            return _mapper.Map<GetQuestionQueryResponse>(question);
        }

        private static Question Find(Question question, Guid id)
        {
            if (question.Id == id)
            {
                return question;
            }

            var nested = question
                .Nodes
                .Cast<Answer>()
                .SelectMany(x => x.Nodes)
                .Where(x => x is Question)
                .Cast<Question>();

            foreach (var q in nested)
            {
                return Find(q, id);
            }

            return null;
        }
    }
}
