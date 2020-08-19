using Story.Application.Domain.Stories.Abstractions;
using System;
using System.Collections.Generic;

namespace Story.Application.Domain.Stories
{
    public sealed class Answer : NodeTree
    {
        public Answer(Guid id, IEnumerable<Question> questions)
          : base(id, questions)
        {
        }

        public Answer(Guid id, TheEnd end)
         : base(id, new[] { end })
        {
        }

        public string Text { get; set; }

        public override NodeLeaf Accept(IVisitor visitor)
        {
            return visitor.VisitAnswer(this);
        }
    }
}
