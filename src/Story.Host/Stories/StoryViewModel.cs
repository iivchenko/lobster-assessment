using System;

namespace Story.Host.Stories
{
    public sealed class StoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RootQuestionId { get; set; }
    }
}
