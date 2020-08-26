using System;

namespace Story.Application.Domain.Polls
{
    public sealed class PollQuestion : PollItem
    {
        public PollQuestion(Guid id, string text) 
            : base(id, text)
        {
        }
    }
}
