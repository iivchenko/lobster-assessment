using System;

namespace Questionnaire.Application.Queries.GetPoll
{
    public sealed class GetPollQueryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RootQuestionId { get; set; }
    }
}
