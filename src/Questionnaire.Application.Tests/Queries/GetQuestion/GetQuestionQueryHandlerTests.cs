using AutoMapper;
using Moq;
using NUnit.Framework;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using Questionnaire.Application.Queries;
using Questionnaire.Application.Queries.GetQuestion;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Tests.Queries.GetQuestion
{
    [TestFixture]
    public sealed class GetQuestionQueryHandlerTests
    {
        private GetQuestionQueryHandler _handler;
        private Mock<IRepository<Poll, Guid>> _pollRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _pollRepository = new Mock<IRepository<Poll, Guid>>();
            _mapper = new Mock<IMapper>();

            _handler = new GetQuestionQueryHandler(_pollRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoPoll_Throws()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var questionId = Guid.NewGuid();

            var query = new GetQuestionQuery
            {
                PollId = pollId,
                QuestionId = questionId
            };

            _pollRepository
                .Setup(x => x.Read(pollId))
                .ReturnsAsync((Poll)null);

            // Act+Assert
            Assert.That(
                async () => await _handler.Handle(query, CancellationToken.None),
                Throws
                    .InstanceOf<EntityNotFoundException>()
                    .With
                    .Property(nameof(EntityNotFoundException.Id))
                    .EqualTo(pollId)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(Poll)));
        }

        [Test]
        public void Handle_NoQuestoin_Throws()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var questionId = Guid.NewGuid();

            var query = new GetQuestionQuery
            {
                PollId = pollId,
                QuestionId = questionId
            };

            var question = new Question(Guid.NewGuid(), "How are you doing?");
            var answer = new Answer(Guid.NewGuid(), "Good");
            var end = new End(Guid.NewGuid(), "Good for you!");

            var items = new PollItem[] { question, answer, end };
            var transitions = new[]
            {
                Transition.Create(question, answer),
                Transition.Create(answer, end)
            };

            var poll = 
                new Poll
                (
                    pollId,
                    "Test",
                    "Just a test poll",
                    question,
                    items,
                    transitions
                );

            _pollRepository
                .Setup(x => x.Read(pollId))
                .ReturnsAsync(poll);

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
                    .EqualTo(nameof(Question)));
        }

        [Test]
        public async Task Handle_HappyPath_Returns()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var questionId = Guid.NewGuid();

            var query = new GetQuestionQuery
            {
                PollId = pollId,
                QuestionId = questionId
            };

            var question = new Question(questionId, "How are you doing?");
            var answer = new Answer(Guid.NewGuid(), "Good");
            var end = new End(Guid.NewGuid(), "Good for you!");

            var items = new PollItem[] { question, answer, end };
            var transitions = new[]
            {
                Transition.Create(question, answer),
                Transition.Create(answer, end)
            };

            var poll =
                new Poll
                (
                    pollId,
                    "Test",
                    "Just a test poll",
                    question,
                    items,
                    transitions
                );

            var expectedRsponse = new GetQuestionQueryResponse
            {
                Id = questionId
            };

            _pollRepository
                .Setup(x => x.Read(pollId))
                .ReturnsAsync(poll);

            _mapper
                .Setup(x => x.Map<GetQuestionQueryResponse>(question))
                .Returns(expectedRsponse);

            //Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Id, Is.EqualTo(questionId));
        }
    }
}
