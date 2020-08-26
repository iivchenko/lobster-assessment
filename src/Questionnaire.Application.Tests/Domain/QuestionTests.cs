using NUnit.Framework;
using Story.Application.Domain.Common;
using Story.Application.Domain.Polls;
using System;

namespace Questionnaire.Application.Tests.Domain
{
    [TestFixture]
    public sealed class QuestionTests
    {
        private const string ProperText = "Some text";

        [Test]
        public void Create_EmptyId_Throws()
        {
            // Arrange+Act+Assert
            Assert
                .That(
                    () => new PollQuestion(Guid.Empty, ProperText),
                    Throws
                        .Exception
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'id' can't be empty!"));
        }

        [Test]
        public void Create_NullText_Throws()
        {
            // Arrange+Act+Assert
            Assert
                .That(
                    () => new PollQuestion(Guid.NewGuid(), null),
                    Throws
                        .Exception
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'text' can't be empty!"));
        }

        [Test]
        public void Create_EmptyText_Throws()
        {
            // Arrange+Act+Assert
            Assert
                .That(
                    () => new PollQuestion(Guid.NewGuid(), string.Empty),
                    Throws
                        .Exception
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'text' can't be empty!"));
        }

        [Test]
        public void Create_AllConditionsMet_CreateNewItem()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Assert
            var question = new PollQuestion(id, ProperText);

            // Act
            Assert.That(question.Id, Is.EqualTo(id));
            Assert.That(question.Text, Is.EqualTo(ProperText));
        }
    }
}
