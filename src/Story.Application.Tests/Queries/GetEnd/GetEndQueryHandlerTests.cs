using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Stories;
using Story.Application.Queries;
using Story.Application.Queries.GetEnd;
using System;
using System.Threading;
using System.Threading.Tasks;
using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Application.Tests.Queries.GetEnd
{
    [TestFixture]
    public sealed class GetEndQueryHandlerTests
    {
        private GetEndQueryHandler _handler;
        private Mock<IStoryRepository> _storyRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _storyRepository = new Mock<IStoryRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetEndQueryHandler(_storyRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoStory_Throws()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            var query = new GetEndQuery
            {
                StoryId = storyId,
                EndId = endId
            };

            _storyRepository
                .Setup(x => x.Read(storyId))
                .ReturnsAsync((AStory)null);

            // Act+Assert
            Assert.That(
                async () => await _handler.Handle(query, CancellationToken.None),
                Throws
                    .InstanceOf<EntityNotFoundException>()
                    .With
                    .Property(nameof(EntityNotFoundException.Id))
                    .EqualTo(storyId)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(Story.Application.Domain.Stories.Story)));
        }

        [Test]
        public void Handle_NoEnd_Throws()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            var query = new GetEndQuery
            {
                StoryId = storyId,
                EndId = endId
            };

            var story =
                CreateStory
                (
                    "test",
                    storyId,
                    CreateQuestion
                    (
                        "question",
                        Guid.NewGuid(),
                        CreateAnswer
                        (
                            "answer",
                            CreateEnd("fake-end")
                        )
                    )
                );

            _storyRepository
                .Setup(x => x.Read(storyId))
                .ReturnsAsync(story);

            // Act+Assert
            Assert.That(
                async () => await _handler.Handle(query, CancellationToken.None),
                Throws
                    .InstanceOf<EntityNotFoundException>()
                    .With
                    .Property(nameof(EntityNotFoundException.Id))
                    .EqualTo(endId)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(TheEnd)));
        }

        [Test]
        public async Task Handle_HappyPath_Returns()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            var query = new GetEndQuery
            {
                StoryId = storyId,
                EndId = endId
            };

            var end = CreateEnd("fake-end", endId);

            var expectedResponse = new GetEndQueryResponse
            {
                Id = endId
            };

            var story =
                CreateStory
                (
                    "test",
                    storyId,
                    CreateQuestion
                    (
                        "question",
                        Guid.NewGuid(),
                        CreateAnswer
                        (
                            "answer",
                            end
                        )
                    )
                );

            _storyRepository
                .Setup(x => x.Read(storyId))
                .ReturnsAsync(story);

            _mapper
              .Setup(x => x.Map<GetEndQueryResponse>(end))
              .Returns(expectedResponse);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Id, Is.EqualTo(endId));
        }


        private static AStory CreateStory(string name, Guid id, Question question)
        {
            return new AStory(
                id,
                name,
                "This is the story of.. some ones life",
                question);
        }

        private static Question CreateQuestion(string text, Guid id, params Answer[] answers)
        {
            return new Question(Guid.NewGuid(), text, answers);
        }

        private static Answer CreateAnswer(string text, TheEnd end)
        {
            return new Answer(Guid.NewGuid(), text, end);
        }

        private static TheEnd CreateEnd(string text)
        {
            return new TheEnd(Guid.NewGuid(), text);
        }

        private static TheEnd CreateEnd(string text, Guid id)
        {
            return new TheEnd(id, text);
        }
    }
}
