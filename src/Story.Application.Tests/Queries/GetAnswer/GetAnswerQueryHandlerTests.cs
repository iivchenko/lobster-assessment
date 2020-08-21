using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Stories;
using Story.Application.Domain.Stories.Abstractions;
using Story.Application.Queries;
using Story.Application.Queries.GetAnswer;
using System;
using System.Threading;
using System.Threading.Tasks;

using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Application.Tests.Queries.GetAnswer
{
    [TestFixture]
    public sealed class GetAnswerQueryHandlerTests
    {
        private GetAnswerQueryHandler _handler;
        private Mock<IStoryRepository> _storyRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _storyRepository = new Mock<IStoryRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetAnswerQueryHandler(_storyRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoStory_Throws()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var answerId = Guid.NewGuid();

            var query = new GetAnswerQuery
            {
                StoryId = storyId,
                AnswerId = answerId
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
        public void Handle_NoAnswer_Throws()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var answerId = Guid.NewGuid();

            var query = new GetAnswerQuery
            {
                StoryId = storyId,
                AnswerId = answerId
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
                            Guid.NewGuid()
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
                    .EqualTo(answerId)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(Answer)));
        }

        [Test]
        public async Task Handle_HappyPath_Returns()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var answerId = Guid.NewGuid();

            var query = new GetAnswerQuery
            {
                StoryId = storyId,
                AnswerId = answerId
            };

            var answer =
                CreateAnswer
                (
                    "correct answer",
                    answerId
                );

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
                            Guid.NewGuid(),
                            CreateQuestion
                            (
                                "question",
                                Guid.NewGuid(),
                                answer
                            )
                        )
                    )
                );

            var expectedRsponse = new GetAnswerQueryResponse
            {
                Id = answerId
            };

            _storyRepository
                .Setup(x => x.Read(storyId))
                .ReturnsAsync(story);

            _mapper
                .Setup(x => x.Map<GetAnswerQueryResponse>(answer))
                .Returns(expectedRsponse);

            //Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Id, Is.EqualTo(answerId));
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
            return new Question(id, text, answers);
        }

        private static Answer CreateAnswer(string text, Guid id, Question question)
        {
            return new Answer(id, text, question);
        }

        private static Answer CreateAnswer(string text, Guid id)
        {
            return new Answer(id, text, NodeLeaf.Empty);
        }
    }
}
