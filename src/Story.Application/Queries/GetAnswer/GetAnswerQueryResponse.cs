using System;
using System.Collections.Generic;

namespace Story.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQueryResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<GetAnswerQueryResponseQuestion> Questions { get; set; }
    }

    public sealed class GetAnswerQueryResponseQuestion
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
