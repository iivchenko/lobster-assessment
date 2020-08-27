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
                    CreateQuestion
                    (
                        "Do I want a Doughnut?",
                        ref transitions,
                        CreateAnswer
                        (
                            "Yes",
                            ref transitions,
                            CreateQuestion
                            (
                                "Do I deserve it?",
                                ref transitions,
                                CreateAnswer
                                (
                                    "Yes",
                                    ref transitions,
                                    CreateQuestion
                                    (
                                        "Are you sure?",
                                        ref transitions,
                                        CreateAnswer
                                        (
                                            "Yes",
                                            ref transitions,
                                            "Get it."
                                        ),
                                        CreateAnswer
                                        (
                                            "No",
                                            ref transitions,
                                            "Do jumping jacks first."
                                        )
                                    )
                                ),
                                CreateAnswer
                                (
                                    "No",
                                    ref transitions,
                                    CreateQuestion
                                    (
                                        "Is it a good doughnut?",
                                        ref transitions,
                                        CreateAnswer
                                        (
                                            "Yes",
                                            ref transitions,
                                            "What are you waiting for? Grab it now."
                                        ),
                                        CreateAnswer
                                        (
                                            "No",
                                            ref transitions,
                                            "Wait til you find a sinful, unforgettable doughnut."
                                        )
                                    )
                                )
                            )
                        ),
                        CreateAnswer
                        (
                            "No",
                            ref transitions,
                            "Maybe you want an apple?"
                        )
                    ),
                    ref transitions
                );
        }

        private static Poll CreatePoll(string name, string description, Question root, ref List<Transition> transitions)
        {
            return new Poll(Guid.NewGuid(), name, description, root, transitions);
        }

        private static Question CreateQuestion(string text, ref List<Transition> transitions, params Answer[] answers)
        {
            var question = new Question(Guid.NewGuid(), text);

            foreach(var answer in answers)
            {
                transitions.Add(Transition.Create(question, answer));
            }

            return question;
        }

        private static Answer CreateAnswer(string text, ref List<Transition> transitions, Question question)
        {
            var answer = new Answer(Guid.NewGuid(), text);

            transitions.Add(Transition.Create(answer, question));

            return answer;
        }

        private static Answer CreateAnswer(string text, ref List<Transition> transitions, string endText)
        {
            var answer = new Answer(Guid.NewGuid(), text);
            var end = new End(Guid.NewGuid(), endText);

            transitions.Add(Transition.Create(answer, end));

            return answer;
        }
    }
}
