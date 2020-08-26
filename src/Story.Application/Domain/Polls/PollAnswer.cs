using System;

namespace Story.Application.Domain.Polls
{
    public sealed class PollAnswer : PollItem
    {
        public PollAnswer(Guid id, string text) 
            : base(id, text)
        {
        }
    }
}
