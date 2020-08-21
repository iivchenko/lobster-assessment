using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Story.Application.Queries.GetAnswer;
using Story.Application.Queries.GetEnd;
using Story.Application.Queries.GetQuestion;
using Story.Application.Queries.GetStories;
using Story.Application.Queries.GetStory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Story.Host.Stories
{
    [Route("api/stories")]
    [ApiController]
    public sealed class StoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public StoriesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<StorySummaryViewModel>> Stories()
        {
            var response = await _mediator.Send(new GetStoriesQuery());

            return _mapper.Map<IEnumerable<StorySummaryViewModel>>(response.Stories);
        }

        [HttpGet("{id}")]
        public async Task<StoryViewModel> Story(Guid id)
        {
            var response = await _mediator.Send(new GetStoryQuery { Id = id } );

            return _mapper.Map<StoryViewModel>(response);
        }

        [HttpGet("{storyId}/questions/{questionId}")]
        public async Task<QuestionViewModel> Question(Guid storyId, Guid questionId)
        {
            var response = await _mediator.Send(new GetQuestionQuery { StoryId = storyId, QuestionId = questionId });

            return _mapper.Map<QuestionViewModel>(response);
        }

        [HttpGet("{storyId}/answers/{answerId}")]
        public async Task<AnswerViewModel> Answer(Guid storyId, Guid answerId)
        {
            var response = await _mediator.Send(new GetAnswerQuery { StoryId = storyId, AnswerId = answerId });

            return _mapper.Map<AnswerViewModel>(response);
        }

        [HttpGet("{storyId}/end/{endId}")]
        public async Task<EndViewModel> End(Guid storyId, Guid endId)
        {
            var response = await _mediator.Send(new GetEndQuery { StoryId = storyId, EndId = endId });

            return _mapper.Map<EndViewModel>(response);
        }
    }
}
