using System;

namespace Questionnaire.Application.Domain.Polls
{
    public sealed class Answer : PollItem
    {
        public Answer(Guid id, string text) 
            : base(id, text)
        {
        }
    }
}
