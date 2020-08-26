using System;
using System.Collections.Generic;

namespace Questionnaire.Host.Polls
{
    public sealed class QuestionViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<AnswerSummaryViewModel> Answers { get; set; }
    }
}
