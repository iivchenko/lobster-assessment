using System;

namespace Story.Host.Stories
{
    public sealed class FullStoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public FullQuestionViewModel Root { get; set; }
    }
}
