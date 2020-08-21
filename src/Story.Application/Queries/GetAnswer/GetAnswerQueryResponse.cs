using System;
using System.Collections.Generic;

namespace Story.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQueryResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public GetAnswerQueryResponseNext Next { get; set; }
    }

    public enum GetAnswerQueryResponseNextType
    {
        Question = 0,
        End = 1
    }

    public sealed class GetAnswerQueryResponseNext
    {
        public Guid Id { get; set; }

        public GetAnswerQueryResponseNextType Type { get; set; }

        public string Text { get; set; }
    }
}
