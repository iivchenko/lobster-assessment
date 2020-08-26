using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Threading.Tasks;

namespace Questionnaire.Host
{
    public static class PollsSeed
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
            var question = new Question(Guid.NewGuid(), "How are you doing?");
            var answer1 = new Answer(Guid.NewGuid(), "Good");
            var answer2 = new Answer(Guid.NewGuid(), "Bad");
            var end1 = new End(Guid.NewGuid(), "Good for you!");
            var end2 = new End(Guid.NewGuid(), "There there");

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
                    question,
                    items,
                    transitions
                );
        }      
    }
}
