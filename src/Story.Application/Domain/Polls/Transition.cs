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
            // check items not null
            // check ids not empty
            return new Transition(Guid.NewGuid(), from.Id, to.Id);
        }

        public static Transition Create(PollAnswer from, PollQuestion to)
        {
            // check items not null
            // check ids not empty
            return new Transition(Guid.NewGuid(), from.Id, to.Id);
        }

        public static Transition Create(PollAnswer from, PollEnd to)
        {
            // check items not null
            // check ids not empty
            return new Transition(Guid.NewGuid(), from.Id, to.Id);
        }
    }
}
