using System;
using System.Collections.Generic;

namespace Story.Application.Queries.GetQuestion
{
    public sealed class GetQuestionResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<GetQuestionResponseAnswer> Answers { get; set; }
    }

    public sealed class GetQuestionResponseAnswer
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
