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
                    var story1 = CreateStory1();
                    var story2 = CreateStory2();

                    repository.Update(story1).GetAwaiter().GetResult();
                    repository.Update(story2).GetAwaiter().GetResult();
                }
            }
        }

        private static AStory CreateStory1()
        {
            return new AStory
            {
                Id = Guid.NewGuid(),
                Name = "Test Story",
                Description = "This is the story of.. some ones life",
                Root = new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "Are you hungry?",
                    Nodes = new[]
                    {
                        new Answer
                        {
                            Id = Guid.NewGuid(),
                            Text = "Yes",
                            Nodes = new[]
                            {
                                new TheEnd
                                {
                                    Id = Guid.NewGuid(),
                                    Message = "You said yes!"
                                }
                            }
                        },
                        new Answer
                        {
                            Id = Guid.NewGuid(),
                            Text = "No",
                            Nodes = new[]
                            {
                                new TheEnd
                                {
                                    Id = Guid.NewGuid(),
                                    Message = "You said no!"
                                }
                            }
                        }
                    }
                }
            };
        }

        private static AStory CreateStory2()
        {
            return new AStory
            {
                Id = Guid.NewGuid(),
                Name = "Test Story 2",
                Description = "This story about story",
                Root = new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "To be or not be?",
                    Nodes = new[]
                    {
                        new Answer
                        {
                            Id = Guid.NewGuid(),
                            Text = "To be",
                            Nodes = new[]
                            {
                                new TheEnd
                                {
                                    Id = Guid.NewGuid(),
                                    Message = "You like Hamlet"
                                }
                            }
                        },
                        new Answer
                        {
                            Id = Guid.NewGuid(),
                            Text = "NO",
                            Nodes = new[]
                            {
                                new TheEnd
                                {
                                    Id = Guid.NewGuid(),
                                    Message = "You don't like Hamlet"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
