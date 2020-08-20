using Story.Application.Domain.Stories.Abstractions;
using System;

namespace Story.Application.Domain.Stories
{
    public sealed class TheEnd : NodeLeaf
    {
        public TheEnd(Guid id, string message)
            : base (id)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
