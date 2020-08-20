using System;

namespace Story.Application.Domain.Stories.Abstractions
{
    public abstract class NodeLeaf
    {
        public static readonly NodeLeaf Empty = new EmptyNode();

        public Guid Id { get; set; }

        public abstract NodeLeaf Accept(IVisitor visitor);

        private sealed class EmptyNode : NodeLeaf
        {
            public override NodeLeaf Accept(IVisitor visitor)
            {
                return this;
            }
        }
    }
}
