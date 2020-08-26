using System;

namespace Story.Host.Polls
{
    public sealed class PollSummaryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
