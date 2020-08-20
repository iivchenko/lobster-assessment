using System;
using System.Collections.Generic;

namespace Story.Application.Domain.Stories.Abstractions
{
    public abstract class NodeTree : NodeLeaf
    {
        public IEnumerable<NodeLeaf> Nodes { get; set; }
    }
}
