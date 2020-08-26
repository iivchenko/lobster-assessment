using System;

namespace Questionnaire.Application.Domain.Polls
{
    public sealed class End : PollItem
    {
        public End(Guid id, string text)
            : base(id, text)
        {
        }
    }
}
