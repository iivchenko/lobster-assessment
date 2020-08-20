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
                    var story = new AStory
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

                    repository.Update(story).GetAwaiter().GetResult();
                }
            }
        }
    }
}
