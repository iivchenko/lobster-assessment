using System;

namespace Story.Application.Queries.GetStory
{
    public sealed class GetStoryQueryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RootQuestionId { get; set; }
    }
}
