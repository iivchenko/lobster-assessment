﻿using Newtonsoft.Json;
using Questionnaire.Application.Domain.Common;
using System;

namespace Questionnaire.Application.Domain.Polls
{
    public sealed class Transition
    {
        [JsonConstructor]
        private Transition(Guid id, PollItem from, PollItem to)
        {
            Id = id;
            From = from;
            To = to;
        }

        public Guid Id { get; private set; }

        public PollItem From { get; private set; }

        public PollItem To { get; private set; }

        public static Transition Create(Question from, Answer to)
        {
            if (from == null)
            {
                throw new DomainException("Question can't be null!");
            }

            if (to == null)
            {
                throw new DomainException("Answer can't be null!");
            }

            return new Transition(Guid.NewGuid(), from, to);
        }

        public static Transition Create(Answer from, Question to)
        {
            if (from == null)
            {
                throw new DomainException("Answer can't be null!");
            }

            if (to == null)
            {
                throw new DomainException("Question can't be null!");
            }

            return new Transition(Guid.NewGuid(), from, to);
        }

        public static Transition Create(Answer from, End to)
        {
            if (from == null)
            {
                throw new DomainException("Answer can't be null!");
            }

            if (to == null)
            {
                throw new DomainException("End can't be null!");
            }

            return new Transition(Guid.NewGuid(), from, to);
        }
    }
}
