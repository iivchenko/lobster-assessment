using System;
using System.Collections.Generic;

namespace Questionnaire.Application.Queries.GetPolls
{
    public sealed class GetPollsQueryResponse
    {
        public IEnumerable<GetPollsQueryPollSummary> Polls { get; set; }
    }

    public sealed class GetPollsQueryPollSummary
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
