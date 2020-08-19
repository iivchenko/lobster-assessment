using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Story.Host.Stories
{
    [Route("api/story")]
    [ApiController]
    public sealed class StoryController : ControllerBase
    {
        [HttpGet]
        public Task<IEnumerable<StorySummaryViewModel>> Stories()
        {
            return Task.FromResult((IEnumerable<StorySummaryViewModel>)null);
        }

        [HttpGet("{id}")]
        public Task<StoryViewModel> Story(Guid id)
        {
            return Task.FromResult((StoryViewModel)null);
        }

        [HttpGet("{storyId}/questions/{questionId}")]
        public Task<QuestionViewModel> Question(Guid storyId, Guid questionId)
        {
            return Task.FromResult((QuestionViewModel)null);
        }

        [HttpGet("{storyId}/answers/{answerId}")]
        public Task<AnswerViewModel> Answer(Guid storyId, Guid answerId)
        {
            return Task.FromResult((AnswerViewModel)null);
        }
    }
}
