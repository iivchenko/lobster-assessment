using Story.Application.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Story.Application.Domain.Polls
{
    public sealed class Poll : IAggregateRoot<Guid>
    {
        public Poll(Guid id, string name, string description, Question rootQuestion, IEnumerable<PollItem> items, IEnumerable<Transition> transitions)
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

            if (rootQuestion == null || rootQuestion.Id == Guid.Empty)
            {
                throw new DomainException($"'{nameof(rootQuestion)}' can't be empty!");
            }

            if (items == null || !items.Any())
            {
                throw new DomainException($"'{nameof(items)}' can't be empty!");
            }

            if (transitions == null || !transitions.Any())
            {
                throw new DomainException($"'{nameof(transitions)}' can't be empty!");
            }

            if (!items.Any(x => x.Id == rootQuestion.Id))
            {
                throw new DomainException($"Root questioin (id: '{rootQuestion.Id}' is not presetn in items!");
            }

            ValidateQuestionTransition(rootQuestion.Id, items, transitions);

            foreach(var item in items)
            {
                switch(item)
                {
                    case Question question:
                        ValidateQuestionTransition(question.Id, items, transitions);
                        break;
                    case Answer answer:
                        ValidateAnswerTransition(answer.Id, items, transitions);
                        break;
                }
            }

            Id = id;
            Name = name;
            Description = description;
            RootQuestion = rootQuestion;

            Items = items;
            Transitions = transitions;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Question RootQuestion { get; private set; }

        public IEnumerable<PollItem> Items { get; private set; }

        public IEnumerable<Transition> Transitions { get; private set; }

        public IEnumerable<Answer> FindNextFor(Question question)
        {
            if (question == null)
            {
                throw new DomainException($"Question should not be null!");
            }

            if (!Items.Any(x => x.Id == question.Id))
            {
                throw new DomainException($"Question (id: '{question.Id}' not present in the Poll!");
            }

            var nextItemIds = 
                Transitions
                    .Where(x => x.FromId == question.Id)
                    .Select(x => x.ToId);

            return Items.Where(x => nextItemIds.Contains(x.Id)).Cast<Answer>();
        }

        public PollItem FindNextFor(Answer answer)
        {
            if (answer == null)
            {
                throw new DomainException($"Answer should not be null!");
            }


            if (!Items.Any(x => x.Id == answer.Id))
            {
                throw new DomainException($"Answer (id: '{answer.Id}' not present in the Poll!");
            }

            var nextItemIds =
                Transitions
                    .Where(x => x.FromId == answer.Id)
                    .Select(x => x.ToId);

            return Items.Single(x => nextItemIds.Contains(x.Id));
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
