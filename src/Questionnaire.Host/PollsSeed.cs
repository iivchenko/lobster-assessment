using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Collections.Generic;
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
                    await repository.Create(CreateDoughnutHuntPoll());
                    await repository.Create(CreateCoffeePoll());
                }
            }
        }

        private static Poll CreateDoughnutHuntPoll()
        {
            var transitions = new List<Transition>();
            return CreatePoll
                (
                    "Doughnut decision helper",
                    "This is not a game! All people in the world needs the guidence on if they want or need a Doughnunt. This guide is about saving your life and preventing wars!",
                    WithQuestion
                    (
                        "Do I want a Doughnut?",
                        ref transitions,
                        WithAnswer
                        (
                            "Yes",
                            ref transitions,
                            WithQuestion
                            (
                                "Do I deserve it?",
                                ref transitions,
                                WithAnswer
                                (
                                    "Yes",
                                    ref transitions,
                                    WithQuestion
                                    (
                                        "Are you sure?",
                                        ref transitions,
                                        WithAnswer
                                        (
                                            "Yes",
                                            ref transitions,
                                            "Get it."
                                        ),
                                        WithAnswer
                                        (
                                            "No",
                                            ref transitions,
                                            "Do jumping jacks first."
                                        )
                                    )
                                ),
                                WithAnswer
                                (
                                    "No",
                                    ref transitions,
                                    WithQuestion
                                    (
                                        "Is it a good doughnut?",
                                        ref transitions,
                                        WithAnswer
                                        (
                                            "Yes",
                                            ref transitions,
                                            "What are you waiting for? Grab it now."
                                        ),
                                        WithAnswer
                                        (
                                            "No",
                                            ref transitions,
                                            "Wait til you find a sinful, unforgettable doughnut."
                                        )
                                    )
                                )
                            )
                        ),
                        WithAnswer
                        (
                            "No",
                            ref transitions,
                            "Maybe you want an apple?"
                        )
                    ),
                    ref transitions
                );
        }

        private static Poll CreateCoffeePoll()
        {
            var transitions = new List<Transition>();
            return CreatePoll
                (
                    "What about morning Coffee",
                    "This simple guide will help you in the morning while your brain still sleep to choose a proper good coffee drink.",
                    WithQuestion
                    (
                        "Good morning! What about strong coffee?",
                        ref transitions,
                        WithAnswer
                        (
                            "Lets have something less strong",
                            ref transitions,
                            WithQuestion
                            (
                                "Any milk?",
                                ref transitions,
                                WithAnswer
                                (
                                    "Of course",
                                    ref transitions,
                                    WithQuestion
                                    (
                                        "Or may be even more milk?",
                                        ref transitions,
                                        WithAnswer
                                        (
                                            "More is better",
                                            ref transitions,
                                            "For today my suggestion for you is Latte. Have a nice drink =)"
                                        ),
                                        WithAnswer
                                        (
                                            "No not today",
                                            ref transitions,
                                            "For today my suggestion for you is Cappuccino. Have a nice drink =)"
                                        )
                                    )
                                ),
                                WithAnswer
                                (
                                    "Am... no... definitely no",
                                    ref transitions,
                                    WithQuestion
                                    (
                                        "Hm... may be some water?",
                                        ref transitions,
                                        WithAnswer
                                        (
                                            "Ho... lets add some water",
                                            ref transitions,
                                             "For today my suggestion for you is Americano. Have a nice drink =)"
                                        ),
                                        WithAnswer
                                        (
                                            "Pff... no... definitely no",
                                            ref transitions,
                                            "For today my suggestion for you is Black Coffee. Have a nice drink =)"
                                        )
                                    )
                                )
                            )
                        ),
                        WithAnswer
                        (
                            "Oh yes please!",
                            ref transitions,
                            WithQuestion
                            (
                                "Some milk?",
                                ref transitions,
                                WithAnswer
                                (
                                    "Nope, just coffee",
                                    ref transitions,
                                    WithQuestion
                                    (
                                        "A bigger cup or smaller?",
                                        ref transitions,
                                        WithAnswer
                                        (
                                            "Just little",
                                            ref transitions,
                                            "For today my suggestion for you is Ristretto. Have a nice drink =)"
                                        ),
                                        WithAnswer
                                        (
                                            "Give me more",
                                            ref transitions,
                                            "For today my suggestion for you is Espresso. Have a nice drink =)"
                                        )
                                    )
                                ),
                                WithAnswer
                                (
                                    "Yeah why not",
                                    ref transitions,
                                    WithQuestion
                                    (
                                        "Or may be foam?",
                                        ref transitions,
                                        WithAnswer
                                        (
                                            "No no just milk",
                                            ref transitions,
                                            "For today my suggestion for you is Flat White. Have a nice drink =)"
                                        ),
                                        WithAnswer
                                        (
                                            "Hm.. why not",
                                            ref transitions,
                                            "For today my suggestion for you is Espresso Machiato. Have a nice drink =)"
                                        )
                                    )
                                )
                            )
                        )
                    ),
                    ref transitions
                );
        }

        private static Poll CreatePoll(string name, string description, Question root, ref List<Transition> transitions)
        {
            return new Poll(Guid.NewGuid(), name, description, root, transitions);
        }

        private static Question WithQuestion(string text, ref List<Transition> transitions, params Answer[] answers)
        {
            var question = new Question(Guid.NewGuid(), text);

            foreach(var answer in answers)
            {
                transitions.Add(Transition.Create(question, answer));
            }

            return question;
        }

        private static Answer WithAnswer(string text, ref List<Transition> transitions, Question question)
        {
            var answer = new Answer(Guid.NewGuid(), text);

            transitions.Add(Transition.Create(answer, question));

            return answer;
        }

        private static Answer WithAnswer(string text, ref List<Transition> transitions, string endText)
        {
            var answer = new Answer(Guid.NewGuid(), text);
            var end = new End(Guid.NewGuid(), endText);

            transitions.Add(Transition.Create(answer, end));

            return answer;
        }
    }
}
