using AutoMapper;
using Moq;
using NUnit.Framework;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using Questionnaire.Application.Queries;
using Questionnaire.Application.Queries.GetEnd;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Tests.Queries.GetEnd
{
    [TestFixture]
    public sealed class GetEndQueryHandlerTests
    {
        private GetEndQueryHandler _handler;
        private Mock<IRepository<Poll, Guid>> _pollRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _pollRepository = new Mock<IRepository<Poll, Guid>>();
            _mapper = new Mock<IMapper>();

            _handler = new GetEndQueryHandler(_pollRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoPoll_Throws()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            var query = new GetEndQuery
            {
                PollId = pollId,
                EndId = endId
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
        public void Handle_NoEnd_Throws()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            var query = new GetEndQuery
            {
                PollId = pollId,
                EndId = endId
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
                    .EqualTo(endId)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(End)));
        }

        [Test]
        public async Task Handle_HappyPath_Returns()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            var query = new GetEndQuery
            {
                PollId = pollId,
                EndId = endId
            };

            var question = new Question(Guid.NewGuid(), "How are you doing?");
            var answer = new Answer(Guid.NewGuid(), "Good");
            var end = new End(endId, "Good for you!");

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

            var expectedResponse = new GetEndQueryResponse
            {
                Id = endId
            };

            _pollRepository
                .Setup(x => x.Read(pollId))
                .ReturnsAsync(poll);

            _mapper
              .Setup(x => x.Map<GetEndQueryResponse>(end))
              .Returns(expectedResponse);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Id, Is.EqualTo(endId));
        }
    }
}
