using Story.Application.Domain.Common;
using System;

namespace Story.Application.Domain.Polls
{
    public abstract class PollItem : IEntity<Guid>
    {
        internal PollItem(Guid id, string text)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException($"'{nameof(id)}' can't be empty!");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new DomainException($"'{nameof(text)}' can't be empty!");
            }

            Id = id;
            Text = text;
        }

        public Guid Id { get; private set; }

        public string Text { get; private set; }
    }
}
