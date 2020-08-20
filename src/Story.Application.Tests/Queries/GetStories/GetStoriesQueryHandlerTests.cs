using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Stories;
using Story.Application.Queries.GetStories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Application.Tests.Queries.GetStories
{
    [TestFixture]
    public sealed class GetStoriesQueryHandlerTests
    {
        private GetStoriesQueryHandler _handler;
        private Mock<IStoryRepository> _storyRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _storyRepository = new Mock<IStoryRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetStoriesQueryHandler(_storyRepository.Object, _mapper.Object);
        }

        [Test]
        public async Task Handle_NoStoriesInTheStorage_ReturnEmpty()
        {
            // Arrange
            var query = new GetStoriesQuery();

            _storyRepository
                .Setup(x => x.ReadAll())
                .ReturnsAsync(Enumerable.Empty<AStory>());

            _mapper
                .Setup(x => x.Map<GetStoryQueryStorySummary>(It.IsAny<AStory>()))
                .Returns(new GetStoryQueryStorySummary());

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Stories, Is.Empty);
        }

        [Test]
        public async Task Handle_DetailedStoriesInTheStorage_ReturnSummary()
        {
            // Arrange
            var query = new GetStoriesQuery();
            var story1 = CreateStory("Story 1");
            var story2 = CreateStory("Story 2");

            _storyRepository
                .Setup(x => x.ReadAll())
                .ReturnsAsync(new[] { story1, story2 });

            _mapper
                .Setup(x => x.Map<GetStoryQueryStorySummary>(story1))
                .Returns(new GetStoryQueryStorySummary { Name = "Story 1 Item" });

            _mapper
                .Setup(x => x.Map<GetStoryQueryStorySummary>(story2))
                .Returns(new GetStoryQueryStorySummary { Name = "Story 2 Item" });

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Stories.Count(), Is.EqualTo(2));
            Assert.That(response.Stories.ElementAt(1).Name, Is.EqualTo("Story 1 Item"));
            Assert.That(response.Stories.ElementAt(2).Name, Is.EqualTo("Story 2 Item"));
        }

        private static AStory CreateStory(string name)
        {
            return new AStory
            {
                Id = Guid.NewGuid(),
                Name = name,
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
    }
}
