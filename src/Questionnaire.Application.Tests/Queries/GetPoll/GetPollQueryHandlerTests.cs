﻿using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Common;
using Story.Application.Domain.Polls;
using Story.Application.Queries;
using Story.Application.Queries.GetPoll;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Story.Application.Tests.Queries.GetPoll
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
            var question = new PollQuestion(Guid.NewGuid(), "How are you doing?");
            var answer1 = new PollAnswer(Guid.NewGuid(), "Good");
            var answer2 = new PollAnswer(Guid.NewGuid(), "Bad");
            var end1 = new PollEnd(Guid.NewGuid(), "Good for you!");
            var end2 = new PollEnd(Guid.NewGuid(), "There there");

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