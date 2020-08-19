using System;
using System.Collections.Generic;

namespace Story.Application.Queries.GetStories
{
    public sealed class GetStoriesQueryResponse
    {
        public IEnumerable<GetStoryQueryStorySummary> Stories { get; set; }
    }

    public sealed class GetStoryQueryStorySummary
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
