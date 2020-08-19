using Story.Application.Domain.Stories.Abstractions;
using System;
using System.Collections.Generic;

namespace Story.Application.Domain.Stories
{
    public sealed class Question : NodeTree
    {
        public Question(Guid id, IEnumerable<Answer> answers)
            : base(id, answers)
        {
        }

        public string Text { get; set; }

        public override NodeLeaf Accept(IVisitor visitor)
        {
            return visitor.VisitQuestion(this);
        }
    }
}
