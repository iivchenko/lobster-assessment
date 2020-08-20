using System;

namespace Story.Application.Domain.Stories
{
    public sealed class Story
    {
        public Story(
            Guid id, 
            string name, 
            string description,
            Question root)
        {
            Id = id;
            Name = name;
            Description = description;
            Root = root;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Question Root { get; private set; }
    }
}
