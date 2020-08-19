using System;
using System.Collections.Generic;

namespace Story.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQueryResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<GetAnswerQueryQuestion> Questions { get; set; }
    }

    public sealed class GetAnswerQueryQuestion
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
