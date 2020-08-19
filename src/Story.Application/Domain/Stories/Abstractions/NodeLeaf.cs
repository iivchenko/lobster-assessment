using System;

namespace Story.Application.Domain.Stories.Abstractions
{
    public abstract class NodeLeaf
    {
        public static readonly NodeLeaf Empty = new EmptyNode();

        internal NodeLeaf(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public abstract NodeLeaf Accept(IVisitor visitor);

        private sealed class EmptyNode : NodeLeaf
        {
            public EmptyNode() 
                : base(Guid.Empty)
            {
            }

            public override NodeLeaf Accept(IVisitor visitor)
            {
                return this;
            }
        }
    }
}
