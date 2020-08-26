using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Story.Application.Domain.Common;
using Story.Application.Domain.Polls;
using System;
using System.Threading.Tasks;

namespace Story.Host
{
    public static class PoolsSeed
    {
        public static async Task Seed(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repository = scope.ServiceProvider.GetService<IRepository<Poll, Guid>>();

                var count = await repository.ReadCount();

                if (count == 0)
                {
                    await repository.Create(TestPoll());

                }
            }
        }

        private static Poll TestPoll()
        {
            var question = new PollQuestion(Guid.NewGuid(), "How are you doing?");
            var answer1 = new PollAnswer(Guid.NewGuid(), "Good");
            var answer2 = new PollAnswer(Guid.NewGuid(), "Bad");
            var end1 = new PollEnd(Guid.NewGuid(), "Good for you!");
            var end2 = new PollEnd(Guid.NewGuid(), "There there");

            var items = new PollItem [] { question, answer1, answer2, end1, end2 };
            var transitions = new[]
            {
                Transition.Create(question, answer1),
                Transition.Create(question, answer2),
                Transition.Create(answer1, end1),
                Transition.Create(answer2, end2),
            };

            return
                new Poll
                (
                    Guid.NewGuid(),
                    "Test",
                    "Just a test poll",
                    question.Id,
                    items,
                    transitions
                );
        }      
    }
}
