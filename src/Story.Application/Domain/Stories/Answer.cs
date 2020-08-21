using Story.Application.Domain.Stories.Abstractions;
using System;

namespace Story.Application.Domain.Stories
{
    public sealed class Answer : NodeLeaf
    {
        public Answer(Guid id, string text, NodeLeaf next)
            : base(id)
        {
            Text = text;
            Next = next;
        }

        public string Text { get; private set; }

        public NodeLeaf Next { get; private set; }
    }
}
