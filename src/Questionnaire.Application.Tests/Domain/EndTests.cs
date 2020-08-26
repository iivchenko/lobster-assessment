using NUnit.Framework;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;

namespace Questionnaire.Application.Tests.Domain
{
    [TestFixture]
    public sealed class EndTests
    {
        private const string ProperText = "Some text";

        [Test]
        public void Create_EmptyId_Throws()
        {
            // Arrange+Act+Assert
            Assert
                .That(
                    () => new End(Guid.Empty, ProperText),
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
                    () => new End(Guid.NewGuid(), null),
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
                    () => new End(Guid.NewGuid(), string.Empty),
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
            var end = new End(id, ProperText);

            // Act
            Assert.That(end.Id, Is.EqualTo(id));
            Assert.That(end.Text, Is.EqualTo(ProperText));
        }
    }
}
