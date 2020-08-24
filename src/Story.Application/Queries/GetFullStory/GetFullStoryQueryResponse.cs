using System;
using System.Collections.Generic;

namespace Story.Application.Queries.GetFullStory
{
    public sealed class GetFullStoryQueryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public GetFullStoryQueryResponseQuestion Root { get; set; }
    }

    public abstract class GetFullStoryQueryResponseAnswerNext
    {
        public Guid Id { get; set; }
    }

    public sealed class GetFullStoryQueryResponseQuestion : GetFullStoryQueryResponseAnswerNext
    {
        public string Text { get; set; }

        public IEnumerable<GetFullStoryQueryResponseAnswer> Answers { get; set; }
    }

    public enum GetFullStoryQueryResponseAnswerNextType
    {
        Question = 0,
        End = 1
    }

    public sealed class GetFullStoryQueryResponseAnswer
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public GetFullStoryQueryResponseAnswerNextType NextType { get; set; }

        public GetFullStoryQueryResponseAnswerNext Next { get; set; }
    }

    public sealed class GetFullStoryQueryResponseEnd : GetFullStoryQueryResponseAnswerNext
    {
        public string Message { get; set; }
    }
}
