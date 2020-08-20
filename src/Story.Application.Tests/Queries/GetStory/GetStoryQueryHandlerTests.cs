using AutoMapper;
using Moq;
using NUnit.Framework;
using Story.Application.Domain.Stories;
using Story.Application.Queries;
using Story.Application.Queries.GetStory;
using System;
using System.Threading;
using System.Threading.Tasks;

using AStory = Story.Application.Domain.Stories.Story;

namespace Story.Application.Tests.Queries.GetStory
{
    [TestFixture]
    public sealed class GetStoryQueryHandlerTests
    {
        private GetStoryQueryHandler _handler;
        private Mock<IStoryRepository> _storyRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _storyRepository = new Mock<IStoryRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetStoryQueryHandler(_storyRepository.Object, _mapper.Object);
        }

        [Test]
        public void Handle_NoStoryInTheStorage_Throws()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetStoryQuery
            {
                Id = id
            };

            _storyRepository
                .Setup(x => x.Read(id))
                .ReturnsAsync((AStory)null);

            // Act+Assert
            Assert.That(
                async () => await _handler.Handle(query, CancellationToken.None),
                Throws
                    .InstanceOf<EntityNotFoundException>()
                    .With
                    .Property("Id")
                    .EqualTo(id));
        }

        [Test]
        public async Task Handle_StoryInTheStorage_Returns()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetStoryQuery
            {
                Id = id
            };

            var story = new AStory();
            var expectedRsponse = new GetStoryQueryResponse();

            _storyRepository
                .Setup(x => x.Read(id))
                .ReturnsAsync(story);

            _mapper
                .Setup(x => x.Map<GetStoryQueryResponse>(story))
                .Returns(expectedRsponse);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(response, Is.EqualTo(expectedRsponse));
        }
    }
}
