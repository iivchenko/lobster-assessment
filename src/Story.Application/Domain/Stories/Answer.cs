using Story.Application.Domain.Stories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Story.Application.Domain.Stories
{
    public sealed class Answer : NodeTree
    {
        public string Text { get; set; }

        public override NodeLeaf Accept(IVisitor visitor)
        {
            return visitor.VisitAnswer(this);
        }
    }
}
