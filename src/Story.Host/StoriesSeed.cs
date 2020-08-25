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
                    repository.Update(CreateCoffeeStory()).GetAwaiter().GetResult();
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
        private static AStory CreateCoffeeStory()
        {
            return new AStory
                (
                    Guid.NewGuid(),
                    "What about morning Coffee",
                    "This simple guide will help you in the morning while your brain still sleep to choose a proper good coffee drink.",
                    CreateQuestion
                    (
                        "Good morning! What about strong coffee?",
                        CreateAnswer
                        (
                            "Lets have something less strong",
                            CreateQuestion
                            (
                                "Any milk?",
                                CreateAnswer
                                (
                                    "Of course",
                                    CreateQuestion
                                    (
                                        "Or may be even more milk?",
                                        CreateAnswer
                                        (
                                            "More is better",
                                            "For today my suggestion for you is Latte. Have a nice drink =)"
                                        ),
                                        CreateAnswer
                                        (
                                            "No not today",
                                            "For today my suggestion for you is Cappuccino. Have a nice drink =)"
                                        )
                                    )
                                ),
                                CreateAnswer
                                (
                                    "Am... no... definitely no",
                                    CreateQuestion
                                    (
                                        "Hm... may be some water?",
                                        CreateAnswer
                                        (
                                            "Ho... lets add some water",
                                             "For today my suggestion for you is Americano. Have a nice drink =)"
                                        ),
                                        CreateAnswer
                                        (
                                            "Pff... no... definitely no",
                                            "For today my suggestion for you is Black Coffee. Have a nice drink =)"
                                        )
                                    )
                                )
                            )
                        ),
                         CreateAnswer
                        (
                            "Oh yes please!",
                            CreateQuestion
                            (
                                "Some milk?",
                                CreateAnswer
                                (
                                    "Nope, just coffee",
                                    CreateQuestion
                                    (
                                        "A bigger cup or smaller?",
                                        CreateAnswer
                                        (
                                            "Just little",
                                            "For today my suggestion for you is Ristretto. Have a nice drink =)"
                                        ),
                                        CreateAnswer
                                        (
                                            "Give me more",
                                            "For today my suggestion for you is Espresso. Have a nice drink =)"
                                        )
                                    )
                                ),
                                CreateAnswer
                                (
                                    "Yeah why not",
                                    CreateQuestion
                                    (
                                        "Or may be foam?",
                                        CreateAnswer
                                        (
                                            "No no just milk",
                                            "For today my suggestion for you is Flat White. Have a nice drink =)"
                                        ),
                                        CreateAnswer
                                        (
                                            "Hm.. why not",
                                            "For today my suggestion for you is Espresso Machiato. Have a nice drink =)"
                                        )
                                    )
                                )
                            )
                        )
                    )
                );
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
