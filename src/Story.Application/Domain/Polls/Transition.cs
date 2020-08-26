using Story.Application.Domain.Common;
using System;

namespace Story.Application.Domain.Polls
{
    public sealed class Transition
    {
        private Transition(Guid id, Guid fromId, Guid toId)
        {
            Id = id;
            FromId = fromId;
            ToId = toId;
        }

        public Guid Id { get; private set; }

        public Guid FromId { get; private set; }

        public Guid ToId { get; private set; }

        public static Transition Create(PollQuestion from, PollAnswer to)
        {
            if (from == null)
            {
                throw new DomainException("Question can't be null!");
            }

            if (to == null)
            {
                throw new DomainException("Answer can't be null!");
            }

            return new Transition(Guid.NewGuid(), from.Id, to.Id);
        }

        public static Transition Create(PollAnswer from, PollQuestion to)
        {
            if (from == null)
            {
                throw new DomainException("Answer can't be null!");
            }

            if (to == null)
            {
                throw new DomainException("Question can't be null!");
            }

            return new Transition(Guid.NewGuid(), from.Id, to.Id);
        }

        public static Transition Create(PollAnswer from, PollEnd to)
        {
            if (from == null)
            {
                throw new DomainException("Answer can't be null!");
            }

            if (to == null)
            {
                throw new DomainException("End can't be null!");
            }

            return new Transition(Guid.NewGuid(), from.Id, to.Id);
        }
    }
}
