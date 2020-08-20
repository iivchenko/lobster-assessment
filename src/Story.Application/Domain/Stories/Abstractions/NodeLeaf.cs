using System;

namespace Story.Application.Domain.Stories.Abstractions
{
    public abstract class NodeLeaf
    {
        public static readonly NodeLeaf Empty = new EmptyNode();

        protected NodeLeaf(Guid id)
        {
            Id = id;
        }       

        public Guid Id { get; private set; }

        private sealed class EmptyNode : NodeLeaf
        {
            public EmptyNode()
                : base(Guid.Empty)
            {
            }
        }
    }
}
