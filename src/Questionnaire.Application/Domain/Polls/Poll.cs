using Newtonsoft.Json;
using Questionnaire.Application.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Questionnaire.Application.Domain.Polls
{
    public sealed class Poll : IAggregateRoot<Guid>
    {
        private readonly ILookup<Guid, Transition> _lookup;

        public Poll(Guid id, string name, string description, Question rootQuestion, IEnumerable<Transition> transitions)
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

            if (transitions == null || !transitions.Any())
            {
                throw new DomainException($"'{nameof(transitions)}' can't be empty!");
            }

            if (!transitions.Any(x => x.From.Id == rootQuestion.Id))
            {
                throw new DomainException($"Root question (id: '{rootQuestion.Id}' is not present in transitions!");
            }

            ValidateTransitions(rootQuestion, transitions);

            var groupFrom = transitions.Select(x => new { id = x.From.Id, transition = x });
            var groupTo = transitions.Select(x => new { id = x.To.Id, transition = x });
            _lookup = groupFrom.Concat(groupTo).ToLookup(key => key.id, value => value.transition);
            
            Id = id;
            Name = name;
            Description = description;
            RootQuestion = rootQuestion;
            Transitions = transitions;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Question RootQuestion { get; private set; }

        [JsonProperty]
        private IEnumerable<Transition> Transitions { get; set; }

        public Question GetQuestion(Guid id)
        {
            if (!_lookup.Contains(id))
            {
                return null;
            }

            var item = _lookup[id].First().From;

            if (!(item is Question question))
            {
                throw new DomainException($"Provided (id: '{id}' present in the Poll but it is not a Question!");
            }

            return question;
        }

        public Answer GetAnswer(Guid id)
        {
            if (!_lookup.Contains(id))
            {
                return null;
            }

            var item = _lookup[id].First().From;

            if (!(item is Answer answer))
            {
                throw new DomainException($"Provided (id: '{id}' present in the Poll but it is not a Answer!");
            }

            return answer;
        }

        public End GetEnd(Guid id)
        {
            if (!_lookup.Contains(id))
            {
                return null;
            }

            var item = _lookup[id].First().To;

            if (!(item is End end))
            {
                throw new DomainException($"Provided (id: '{id}' present in the Poll but it is not an End!");
            }

            return end;
        }

        public IEnumerable<Answer> FindNextFor(Question question)
        {
            if (question == null)
            {
                throw new DomainException($"Question should not be null!");
            }

            if(!_lookup.Contains(question.Id))
            {
                throw new DomainException($"Question (id: '{question.Id}' not present in the Poll!");
            }

            return _lookup[question.Id].Where(x => x.From.Id == question.Id).Select(x => x.To).Cast<Answer>();
        }

        public PollItem FindNextFor(Answer answer)
        {
            if (answer == null)
            {
                throw new DomainException($"Answer should not be null!");
            }

            if (!_lookup.Contains(answer.Id))
            {
                throw new DomainException($"Answer (id: '{answer.Id}' not present in the Poll!");
            }

            return _lookup[answer.Id].Single(x => x.From.Id == answer.Id).To;
        }       

        private static void ValidateTransitions(Question root, IEnumerable<Transition> transitions)
        {
            var transitionTo =
               transitions
                   .Where(x => !(x.To is End))
                   .Select(x => x.To);

            foreach (var transition in transitionTo)
            {
                if (!transitions.Any(y => y.From.Id == transition.Id))
                {
                    throw new DomainException($"{transition.GetType().Name} (id: '{transition.Id}' misses transition To!");
                }
            }

            var transitionsFrom =
               transitions
                   .Where(x => x.From.Id != root.Id)
                   .Where(x => !(x.From is End))
                   .Select(x => x.From);

            foreach (var transition in transitionsFrom)
            {
                if (!transitions.Any(y => y.To.Id == transition.Id))
                {
                    throw new DomainException($"{transition.GetType().Name} (id: '{transition.Id}' misses transition From!");
                }
            }
        }
    }
}
