using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Story.Application.Queries.GetPolls;
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
    }

    public sealed class PollSummaryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
