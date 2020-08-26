using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Story.Application.Queries.GetPoll;
using Story.Application.Queries.GetPolls;
using Story.Application.Queries.GetQuestion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Story.Host.Polls
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
        public async Task<QuestionViewModel> Question(Guid pollId, Guid questionId)
        {
            var response = await _mediator.Send(new GetQuestionQuery { PollId = pollId, QuestionId = questionId });

            return _mapper.Map<QuestionViewModel>(response);
        }
    }

    public sealed class PollViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RootQuestionId { get; set; }
    }

    public sealed class PollSummaryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public sealed class QuestionViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<AnswerSummaryViewModel> Answers { get; set; }
    }

    public sealed class AnswerSummaryViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
