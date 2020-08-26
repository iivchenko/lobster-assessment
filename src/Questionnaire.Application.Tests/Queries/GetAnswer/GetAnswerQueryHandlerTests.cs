using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Common;
using Story.Application.Domain.Polls;
using Story.Application.Queries;
using Story.Application.Queries.GetAnswer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Tests.Queries.GetAnswer
{
    [TestFixture]
    public sealed class GetAnswerQueryHandlerTests
    {
        private GetAnswerQueryHandler _handler;
        private Mock<IRepository<Poll, Guid>> _pollRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _pollRepository = new Mock<IRepository<Poll, Guid>>();
            _mapper = new Mock<IMapper>();

            _handler = new GetAnswerQueryHandler(_pollRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoPoll_Throws()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var answerId = Guid.NewGuid();

            var query = new GetAnswerQuery
            {
                PollId = pollId,
                AnswerId = answerId
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
        public void Handle_NoAnswer_Throws()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var answerId = Guid.NewGuid();

            var query = new GetAnswerQuery
            {
                PollId = pollId,
                AnswerId = answerId
            };

            var question = new PollQuestion(Guid.NewGuid(), "How are you doing?");
            var answer = new PollAnswer(Guid.NewGuid(), "Good");
            var end = new PollEnd(Guid.NewGuid(), "Good for you!");

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
                    .EqualTo(answerId)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(PollAnswer)));
        }

        [Test]
        public async Task Handle_HappyPath_Returns()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var answerId = Guid.NewGuid();

            var query = new GetAnswerQuery
            {
                PollId = pollId,
                AnswerId = answerId
            };

            var question = new PollQuestion(Guid.NewGuid(), "How are you doing?");
            var answer = new PollAnswer(answerId, "Good");
            var end = new PollEnd(Guid.NewGuid(), "Good for you!");

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

            var expectedRsponse = new GetAnswerQueryResponse
            {
                Id = answerId
            };

            _pollRepository
                .Setup(x => x.Read(pollId))
                .ReturnsAsync(poll);

            _mapper
                .Setup(x => x.Map<GetAnswerQueryResponse>(answer))
                .Returns(expectedRsponse);

            //Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Id, Is.EqualTo(answerId));
        }
    }
}
