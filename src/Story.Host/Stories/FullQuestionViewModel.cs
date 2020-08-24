using System.Collections.Generic;

namespace Story.Host.Stories
{
    public sealed class FullQuestionViewModel : FullAnswerNextViewModel
    {
        public string Text { get; set; }

        public IEnumerable<FullAnswerViewModel> Answers { get; set; }
    }
}
