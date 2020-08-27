using AutoMapper;
using Moq;
using NUnit.Framework;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using Questionnaire.Application.Queries;
using Questionnaire.Application.Queries.GetAnswer;
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

            var question = new Question(Guid.NewGuid(), "How are you doing?");
            var answer = new Answer(Guid.NewGuid(), "Good");
            var end = new End(Guid.NewGuid(), "Good for you!");

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
                    .EqualTo(nameof(Answer)));
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

            var question = new Question(Guid.NewGuid(), "How are you doing?");
            var answer = new Answer(answerId, "Good");
            var end = new End(Guid.NewGuid(), "Good for you!");

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
