using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Common;
using Story.Application.Domain.Polls;
using Story.Application.Queries.GetPolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Application.Tests.Queries.GetPolls
{
    [TestFixture]
    public sealed class GetPollsQueryHandlerTests
    {
        private GetPollsQueryHandler _handler;
        private Mock<IRepository<Poll, Guid>> _pollRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _pollRepository = new Mock<IRepository<Poll, Guid>>();
            _mapper = new Mock<IMapper>();

            _handler = new GetPollsQueryHandler(_pollRepository.Object, _mapper.Object);
        }

        [Test]
        public async Task Handle_NoPollsInTheStorage_ReturnEmpty()
        {
            // Arrange
            var query = new GetPollsQuery();

            _pollRepository
                .Setup(x => x.ReadAll(-1, -1))
                .ReturnsAsync(Enumerable.Empty<Poll>());

            _mapper
                .Setup(x => x.Map<GetPollsQueryPollSummary>(It.IsAny<Poll>()))
                .Returns(new GetPollsQueryPollSummary());

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Polls, Is.Empty);
        }

        [Test]
        public async Task Handle_DetailedPollsInTheStorage_ReturnSummary()
        {
            // Arrange
            var query = new GetPollsQuery();
            var poll1 = CreatePoll();
            var poll2 = CreatePoll();
            var polls = new[] { poll1, poll2 };

            var expectedSummaries = new[]
            {
                new GetPollsQueryPollSummary { Name = "Poll 1 Item" },
                new GetPollsQueryPollSummary { Name = "Poll 2 Item" }
            };

            _pollRepository
                .Setup(x => x.ReadCount())
                .ReturnsAsync(2); 

            _pollRepository
                .Setup(x => x.ReadAll(0, 2))
                .ReturnsAsync(new[] { poll1, poll2 });

            _mapper
                .Setup(x => x.Map<IEnumerable<GetPollsQueryPollSummary>>(polls))
                .Returns(expectedSummaries);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response.Polls.Count(), Is.EqualTo(2));
            Assert.That(response.Polls.ElementAt(0).Name, Is.EqualTo("Poll 1 Item"));
            Assert.That(response.Polls.ElementAt(1).Name, Is.EqualTo("Poll 2 Item"));
        }

        private static Poll CreatePoll()
        {
            var question = new Question(Guid.NewGuid(), "How are you doing?");
            var answer1 = new Answer(Guid.NewGuid(), "Good");
            var answer2 = new Answer(Guid.NewGuid(), "Bad");
            var end1 = new End(Guid.NewGuid(), "Good for you!");
            var end2 = new End(Guid.NewGuid(), "There there");

            var items = new PollItem[] { question, answer1, answer2, end1, end2 };
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
                    items,
                    transitions
                );
        }
    }
}
