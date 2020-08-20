using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Stories;
using Story.Application.Queries;
using Story.Application.Queries.GetQuestion;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Application.Tests.Queries.GetQuestion
{
    [TestFixture]
    public sealed class GetQuestionQueryHandlerTests
    {
        private GetQuestionQueryHandler _handler;
        private Mock<IStoryRepository> _storyRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _storyRepository = new Mock<IStoryRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetQuestionQueryHandler(_storyRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoStory_Throws()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var questionId = Guid.NewGuid();

            var query = new GetQuestionQuery
            {
                StoryId = storyId,
                QuestionId = questionId
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
        public void Handle_NoQuestoin_Throws()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var questionId = Guid.NewGuid();

            var query = new GetQuestionQuery
            {
                StoryId = storyId,
                QuestionId = questionId
            };

            var story = CreateStory("test", storyId, CreateQuestion("question", Guid.NewGuid()));

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
                    .EqualTo(questionId)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(Story.Application.Domain.Stories.Question)));
        }

        [Test]
        public async Task Handle_HappyPath_Returns()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            var questionId = Guid.NewGuid();

            var query = new GetQuestionQuery
            {
                StoryId = storyId,
                QuestionId = questionId
            };

            var question = CreateQuestion("question", storyId);
            var story = CreateStory("test", storyId, question);

            var expectedRsponse = new GetQuestionResponse
            {
                Id = questionId
            };

            _storyRepository
                .Setup(x => x.Read(storyId))
                .ReturnsAsync(story);

            _mapper
                .Setup(x => x.Map<GetQuestionResponse>(question))
                .Returns(expectedRsponse);

            //Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Id, Is.EqualTo(questionId));
        }

        private static AStory CreateStory(string name, Guid id, Question question)
        {
            return new AStory(
                id,
                name,
                "This is the story of.. some ones life",
                question);
        }

        private static Question CreateQuestion(string text, Guid id)
        {
            return new Question(id, text, Enumerable.Empty<Answer>());
        }
    }
}
