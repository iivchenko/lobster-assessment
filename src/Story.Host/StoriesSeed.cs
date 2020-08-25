using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Story.Application.Domain.Stories;
using System;
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
                    repository.Update(CreateDoughnutHuntStory()).GetAwaiter().GetResult();
                    repository.Update(CreateStory1()).GetAwaiter().GetResult();
                }
            }
        }

        private static AStory CreateDoughnutHuntStory()
        {
            return 
                new AStory
                (
                    Guid.NewGuid(),
                    "Doughnut decision helper",
                    "This is not a game! All people in the world needs the guidence on if they want or need a Doughnunt. This guide is about saving your life and preventing wars!",
                    CreateQuestion
                    (
                        "Do I want a Doughnut?",
                        CreateAnswer
                        (
                            "Yes",
                            CreateQuestion
                            (
                                "Do I deserve it?",
                                CreateAnswer
                                (
                                    "Yes",
                                    CreateQuestion
                                    (
                                        "Are you sure?",
                                        CreateAnswer
                                        (
                                            "Yes",
                                            "Get it."
                                        ),
                                        CreateAnswer
                                        (
                                            "No",
                                            "Do jumping jacks first."
                                        )
                                    )
                                ),
                                CreateAnswer
                                (
                                    "No",
                                    CreateQuestion
                                    (
                                        "Is it a good doughnut?",
                                        CreateAnswer
                                        (
                                            "Yes",
                                            "What are you waiting for? Grab it now."
                                        ),
                                        CreateAnswer
                                        (
                                            "No",
                                            "Wait til you find a sinful, unforgettable doughnut."
                                        )
                                    )
                                )
                            )
                        ),
                        CreateAnswer
                        (
                            "No",
                            "Maybe you want an apple?"
                        )
                    )
                );
        }
        private static AStory CreateStory1()
        {
            return new AStory
                (
                    Guid.NewGuid(),
                    "Test Story",
                    "This is the story of.. some ones life",
                    CreateQuestion
                    (
                        "Are you hungry?",
                        CreateAnswer
                        (
                            "Yes",
                            CreateQuestion
                            (
                                "Are you sure?",
                                CreateAnswer("Yes", "Then eat something."),
                                CreateAnswer("No", "So go and do some job.")
                            )
                        ),
                        CreateAnswer("No", "So go and do some job.")
                ));
        }

        private static Question CreateQuestion(string text, params Answer[] answers)
        {
            return new Question(Guid.NewGuid(), text, answers);
        }

        private static Answer CreateAnswer(string text, string end)
        {
            return new Answer(Guid.NewGuid(), text, new TheEnd(Guid.NewGuid(), end));
        }

        private static Answer CreateAnswer(string text, Question question)
        {
            return new Answer(Guid.NewGuid(), text, question);
        }
    }
}
