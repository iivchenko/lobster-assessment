using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Stories;
using Story.Application.Queries.GetStories;
using System;
using System.Collections.Generic;
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
            var stories = new[] { story1, story2 };

            var expectedSummaries = new []
            {
                new GetStoryQueryStorySummary { Name = "Story 1 Item" },
                new GetStoryQueryStorySummary { Name = "Story 2 Item" }
            };

            _storyRepository
                .Setup(x => x.ReadAll())
                .ReturnsAsync(new[] { story1, story2 });

            _mapper
                .Setup(x => x.Map<IEnumerable<GetStoryQueryStorySummary>>(stories))
                .Returns(expectedSummaries);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Stories.Count(), Is.EqualTo(2));
            Assert.That(response.Stories.ElementAt(0).Name, Is.EqualTo("Story 1 Item"));
            Assert.That(response.Stories.ElementAt(1).Name, Is.EqualTo("Story 2 Item"));
        }

        private static AStory CreateStory(string name)
        {
            return new AStory(
                Guid.NewGuid(),
                name,
                "This is the story of.. some ones life",
                CreateQuestion(
                    "Are you hungry?",
                    CreateAnswer("Yes", "You said yes!"),
                    CreateAnswer("No", "You said no!")
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
    }
}
