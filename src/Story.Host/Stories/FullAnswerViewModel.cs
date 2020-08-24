using System;

namespace Story.Host.Stories
{
    public sealed class FullAnswerViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public FullAnswerNextType NextType { get; set; }

        public FullAnswerNextViewModel Next { get; set; }
    }
}
