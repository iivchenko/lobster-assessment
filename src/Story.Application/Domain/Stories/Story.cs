using System;

namespace Story.Application.Domain.Stories
{
    public sealed class Story
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Question Root { get; set; }
    }
}
