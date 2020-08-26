using Story.Application.Domain.Common;
using System;
using System.Collections.Generic;

namespace Story.Application.Domain.Polls
{
    public sealed class Poll : IAggregateRoot<Guid>
    {
        private readonly IEnumerable<PollItem> _items;
        private readonly IEnumerable<Transition> _transitions;

        public Poll(Guid id, Guid rootQuestionId, IEnumerable<PollItem> items, IEnumerable<Transition> transitions)
        {
            // check items has rootQuestionId
            // check all items used property in transtitions
            // check transition has correct directions
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public PollQuestion RootQuestion { get; private set; }

        public IEnumerable<PollAnswer> FindNextFor(PollQuestion question)
        {
            // check quesiton exists in items if not return null
            // check find transition and return
            return null;
        }

        public IEnumerable<PollItem> FindNextFor(PollAnswer answer)
        {
            // check answer exists in items if not return null
            // check find transition and return
            return null;
        }
    }
}
