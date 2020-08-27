using AutoMapper;
using Moq;
using NUnit.Framework;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using Questionnaire.Application.Queries;
using Questionnaire.Application.Queries.GetPoll;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Tests.Queries.GetPoll
{
    [TestFixture]
    public sealed class GetPollQueryHandlerTests
    {
        private GetPollQueryHandler _handler;
        private Mock<IRepository<Poll, Guid>> _pollRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _pollRepository = new Mock<IRepository<Poll, Guid>>();
            _mapper = new Mock<IMapper>();

            _handler = new GetPollQueryHandler(_pollRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoPollInTheStorage_Throws()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetPollQuery
            {
                Id = id
            };

            _pollRepository
                .Setup(x => x.Read(id))
                .ReturnsAsync((Poll)null);

            // Act+Assert
            Assert.That(
                async () => await _handler.Handle(query, CancellationToken.None),
                Throws
                    .InstanceOf<EntityNotFoundException>()
                    .With
                    .Property(nameof(EntityNotFoundException.Id))
                    .EqualTo(id)
                    .And
                    .Property(nameof(EntityNotFoundException.Type))
                    .EqualTo(nameof(Poll)));
        }

        [Test]
        public async Task Handle_PollInTheStorage_Returns()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetPollQuery
            {
                Id = id
            };

            var poll = CreatePoll();
            var expectedRsponse = new GetPollQueryResponse();

            _pollRepository
                .Setup(x => x.Read(id))
                .ReturnsAsync(poll);

            _mapper
                .Setup(x => x.Map<GetPollQueryResponse>(poll))
                .Returns(expectedRsponse);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response, Is.EqualTo(expectedRsponse));
        }

        private static Poll CreatePoll()
        {
            var question = new Question(Guid.NewGuid(), "How are you doing?");
            var answer1 = new Answer(Guid.NewGuid(), "Good");
            var answer2 = new Answer(Guid.NewGuid(), "Bad");
            var end1 = new End(Guid.NewGuid(), "Good for you!");
            var end2 = new End(Guid.NewGuid(), "There there");

            var transitions = new[]
            {
                Transition.Create(question, answer1),
                Transition.Create(question, answer2),
                Transition.Create(answer1, end1),
                Transition.Create(answer2, end2),
            };

            return
                new Poll
                (
                    Guid.NewGuid(),
                    "Test",
                    "Just a test poll",
                    question,
                    transitions
                );
        }
    }
}
