using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Story.Application.Queries.GetAnswer;
using Story.Application.Queries.GetEnd;
using Story.Application.Queries.GetFullStory;
using Story.Application.Queries.GetQuestion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Story.Host.Stories
{
    [Route("api/stories")]
    [ApiController]
    public sealed class PollsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PollsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id}/full")]
        public async Task<FullStoryViewModel> StoryFull(Guid id)
        {
            var response = await _mediator.Send(new GetFullStoryQuery { Id = id });

            return _mapper.Map<FullStoryViewModel>(response);
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
