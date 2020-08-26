using System;
using System.Collections.Generic;

namespace Questionnaire.Application.Queries.GetQuestion
{
    public sealed class GetQuestionQueryResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<GetQuestionQueryResponseAnswer> Answers { get; set; }
    }

    public sealed class GetQuestionQueryResponseAnswer
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
