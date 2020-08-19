using Story.Application.Domain.Stories.Abstractions;
using System;

namespace Story.Application.Domain.Stories
{
    public sealed class TheEnd : NodeLeaf
    {
        public TheEnd(Guid id)
            : base(id)
        {
        }

        public string Message { get; set; }

        public override NodeLeaf Accept(IVisitor visitor)
        {
            return visitor.VisitTheEnd(this);
        }
    }
}
