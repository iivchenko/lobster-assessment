﻿using Story.Application.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Story.Application.Domain.Polls
{
    public sealed class Poll : IAggregateRoot<Guid>
    {
        [JsonProperty] private readonly IEnumerable<PollItem> _items;
        [JsonProperty] private readonly IEnumerable<Transition> _transitions;

        public Poll(Guid id, string name, string description, Guid rootQuestionId, IEnumerable<PollItem> items, IEnumerable<Transition> transitions)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException($"'{nameof(id)}' can't be empty!");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException($"'{nameof(name)}' can't be empty!");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException($"'{nameof(description)}' can't be empty!");
            }

            if (rootQuestionId == Guid.Empty)
            {
                throw new DomainException($"'{nameof(rootQuestionId)}' can't be empty!");
            }

            if (items == null || !items.Any())
            {
                throw new DomainException($"'{nameof(items)}' can't be empty!");
            }

            if (transitions == null || !transitions.Any())
            {
                throw new DomainException($"'{nameof(transitions)}' can't be empty!");
            }

            if (!items.Any(x => x.Id == rootQuestionId))
            {
                throw new DomainException($"Root questioin (id: '{rootQuestionId}' is not presetn in items!");
            }

            ValidateQuestionTransition(rootQuestionId, items, transitions);

            foreach(var item in items)
            {
                switch(item)
                {
                    case PollQuestion question:
                        ValidateQuestionTransition(question.Id, items, transitions);
                        break;
                    case PollAnswer answer:
                        ValidateAnswerTransition(answer.Id, items, transitions);
                        break;
                }
            }

            Id = id;
            Name = name;
            Description = description;
            RootQuestion = (PollQuestion)items.Single(x => x.Id == rootQuestionId);

            _items = items;
            _transitions = transitions;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public PollQuestion RootQuestion { get; private set; }

        public IEnumerable<PollAnswer> FindNextFor(PollQuestion question)
        {
            if (question == null)
            {
                throw new DomainException($"Question should not be null!");
            }

            if (!_items.Any(x => x.Id == question.Id))
            {
                throw new DomainException($"Question (id: '{question.Id}' not present in the Poll!");
            }

            var nextItemIds = 
                _transitions
                    .Where(x => x.FromId == question.Id)
                    .Select(x => x.ToId);

            return _items.Where(x => nextItemIds.Contains(x.Id)).Cast<PollAnswer>();
        }

        public PollItem FindNextFor(PollAnswer answer)
        {
            if (answer == null)
            {
                throw new DomainException($"Answer should not be null!");
            }


            if (!_items.Any(x => x.Id == answer.Id))
            {
                throw new DomainException($"Answer (id: '{answer.Id}' not present in the Poll!");
            }

            var nextItemIds =
                _transitions
                    .Where(x => x.FromId == answer.Id)
                    .Select(x => x.ToId);

            return _items.Single(x => nextItemIds.Contains(x.Id));
        }

        private static void ValidateQuestionTransition(Guid id, IEnumerable<PollItem> items, IEnumerable<Transition> transitions)
        {
            var questionTransitions = transitions.Where(x => x.FromId == id);

            if (!questionTransitions.Any())
            {
                throw new DomainException($"Question (id: '{id}' should have transition!");
            }
        }

        private static void ValidateAnswerTransition(Guid id, IEnumerable<PollItem> items, IEnumerable<Transition> transitions)
        {
            var answerTransitions = transitions.Where(x => x.FromId == id);

            if (!answerTransitions.Any())
            {
                throw new DomainException($"Answer (id: '{id}' should have transition!");
            }
        }
    }
}
