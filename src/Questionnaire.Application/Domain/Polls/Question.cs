using System;

namespace Questionnaire.Application.Domain.Polls
{
    public sealed class Question : PollItem
    {
        public Question(Guid id, string text) 
            : base(id, text)
        {
        }
    }
}
