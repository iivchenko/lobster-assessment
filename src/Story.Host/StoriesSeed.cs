using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Story.Application.Domain.Stories;
using System;
using System.Collections.Generic;
using System.Linq;

using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Host
{
    public static class StoriesSeed
    {
        public static void Seed(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repository = scope.ServiceProvider.GetService<IStoryRepository>();

                var stories = repository.ReadAll().GetAwaiter().GetResult().ToList();

                if (!stories.Any())
                {
                    repository.Update(CreateStory1()).GetAwaiter().GetResult();
                    repository.Update(CreateStory2()).GetAwaiter().GetResult();
                }
            }
        }

        private static AStory CreateStory1()
        {
            return new AStory(
                Guid.NewGuid(),
                "Test Story",
                "This is the story of.. some ones life",
                CreateQuestion(
                    "Are you hungry?",
                    CreateAnswer("Yes", "You said yes!"),
                    CreateAnswer("No", "You said no!")
                ));
        }

        private static AStory CreateStory2()
        {
            return new AStory(
               Guid.NewGuid(),
               "Test Story 2",
               "This story about story",
               CreateQuestion(
                   "To be or not be?",
                   CreateAnswer("To be", "You like Hamlet"),
                   CreateAnswer("Not to be", "Why don't you like Hamlet?")
               ));
        }

        private static Question CreateQuestion(string text, params Answer[] answers)
        {
            return new Question(Guid.NewGuid(), text, answers);
        }

        private static Answer CreateAnswer(string text, string end)
        {
            return new Answer(Guid.NewGuid(), text, new[] { new TheEnd(Guid.NewGuid(), end) });
        }

        private static Answer CreateAnswer(string text, IEnumerable<Question> questions)
        {
            return new Answer(Guid.NewGuid(), text, questions);
        }
    }
}
