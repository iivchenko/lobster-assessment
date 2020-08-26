using System;

namespace Questionnaire.Host.Polls
{
    public sealed class PollViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RootQuestionId { get; set; }
    }
}
