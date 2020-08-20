using Story.Application.Domain.Stories.Abstractions;
using System;
using System.Collections.Generic;

namespace Story.Application.Domain.Stories
{
    public sealed class Answer : NodeTree
    {
        public Answer(Guid id, string text, IEnumerable<NodeLeaf> nodes)
            : base(id, nodes)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}
