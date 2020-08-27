using System;

namespace Questionnaire.Host.Polls
{
    public sealed class AnswerViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public EntityType NextEntityType { get; set; }

        public Guid NextEntityId { get; set; }

        public enum EntityType
        {
            Question,
            End
        }
    }
}
