using System;

namespace Story.Host.Stories
{
    public enum NextEntityType
    {
        Question,
        End
    }

    public sealed class AnswerViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
       
        public NextEntityType NextEntityType { get; set; }

        public Guid NextEntityId { get; set; }
    }
}
