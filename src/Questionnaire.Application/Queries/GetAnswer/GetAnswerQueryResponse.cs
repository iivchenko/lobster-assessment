using System;

namespace Questionnaire.Application.Queries.GetAnswer
{
    public sealed class GetAnswerQueryResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public GetAnswerQueryResponseNext Next { get; set; }
    }

    public abstract class GetAnswerQueryResponseNext
    {
        public Guid Id { get; set; }
    }

    public sealed class GetAnswerQueryResponseNextQuestion : GetAnswerQueryResponseNext
    {
    }

    public sealed class GetAnswerQueryResponseNextEnd : GetAnswerQueryResponseNext
    {
    }
}
