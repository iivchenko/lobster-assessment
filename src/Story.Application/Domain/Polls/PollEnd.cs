using System;

namespace Story.Application.Domain.Polls
{
    public sealed class PollEnd : PollItem
    {
        public PollEnd(Guid id, string text)
            : base(id, text)
        {
        }
    }
}
