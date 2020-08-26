using Story.Application.Domain.Common;
using System;

namespace Story.Application.Domain.Polls
{
    public abstract class PollItem : IEntity<Guid>
    {
        internal PollItem(Guid id, string text)
        {
            // check id not empty 
            // check text not empty
            Id = id;
            Text = text;
        }

        public Guid Id { get; private set; }

        public string Text { get; private set; }
    }
}
