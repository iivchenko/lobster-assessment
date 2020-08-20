using System;
using System.Collections.Generic;

namespace Story.Application.Domain.Stories.Abstractions
{
    public abstract class NodeTree : NodeLeaf
    {
        protected NodeTree(Guid id, IEnumerable<NodeLeaf> nodes)
            : base(id)
        {
            Nodes = nodes;
        }

        public IEnumerable<NodeLeaf> Nodes { get; private set; }
    }
}
