using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Application.Queries.GetAnswer;
using Questionnaire.Application.Queries.GetEnd;
using Questionnaire.Application.Queries.GetPoll;
using Questionnaire.Application.Queries.GetPolls;
using Questionnaire.Application.Queries.GetQuestion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questionnaire.Host.Polls
{
    [Route("api/polls")]
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

        [HttpGet]
        public async Task<IEnumerable<PollSummaryViewModel>> GetPolls()
        {
            var response = await _mediator.Send(new GetPollsQuery());

            return _mapper.Map<IEnumerable<PollSummaryViewModel>>(response.Polls);
        }

        [HttpGet("{id}")]
        public async Task<PollViewModel> GetPoll(Guid id)
        {
            var response = await _mediator.Send(new GetPollQuery { Id = id });

            return _mapper.Map<PollViewModel>(response);
        }

        [HttpGet("{pollId}/questions/{questionId}")]
        public async Task<QuestionViewModel> GetQuestion(Guid pollId, Guid questionId)
        {
            var response = await _mediator.Send(new GetQuestionQuery { PollId = pollId, QuestionId = questionId });

            return _mapper.Map<QuestionViewModel>(response);
        }

        [HttpGet("{pollId}/answers/{answerId}")]
        public async Task<AnswerViewModel> GetAnswer(Guid pollId, Guid answerId)
        {
            var response = await _mediator.Send(new GetAnswerQuery { PollId = pollId, AnswerId = answerId });

            return _mapper.Map<AnswerViewModel>(response);
        }

        [HttpGet("{pollId}/end/{endId}")]
        public async Task<EndViewModel> End(Guid pollId, Guid endId)
        {
            var response = await _mediator.Send(new GetEndQuery { PollId = pollId, EndId = endId });

            return _mapper.Map<EndViewModel>(response);
        }
    }
}
