using AutoMapper;
using MediatR;
using Story.Application.Domain.Stories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, GetQuestionResponse>
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

        public Task<GetQuestionResponse> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {
            // get story by id
            // if no story - throws
            // find question by id 
            // if no questio - throws
            // map and return question
            throw new NotImplementedException();
        }
    }
}
