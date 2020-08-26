using NUnit.Framework;
using Story.Application.Domain.Common;
using Story.Application.Domain.Polls;
using System;

namespace Questionnaire.Application.Tests.Domain
{
    [TestFixture]
    public sealed class TransitionTests
    {
        private const string ProperText = "Some text";

        [Test]
        public void Create_FromQuestionToAnswer_EmptyQuestion_Throws()
        {
            // Arrange
            var question = (Question)null;
            var answer = new Answer(Guid.NewGuid(), ProperText);

            // Act + Assert
            Assert
                .That(
                    () => Transition.Create(question, answer),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("Question can't be null!"));
        }

        [Test]
        public void Create_FromQuestionToAnswer_EmptyAnswer_Throws()
        {
            // Arrange
            var question = new Question(Guid.NewGuid(), ProperText);
            var answer = (Answer)null;

            // Act + Assert
            Assert
                .That(
                    () => Transition.Create(question, answer),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("Answer can't be null!"));
        }

        [Test]
        public void Create_FromAnswerToQuestion_EmptyQuestion_Throws()
        {
            // Arrange
            var question = (Question)null;
            var answer = new Answer(Guid.NewGuid(), ProperText);

            // Act + Assert
            Assert
                .That(
                    () => Transition.Create(answer, question),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("Question can't be null!"));
        }

        [Test]
        public void Create_FromAnswerToQuestion_EmptyAnswer_Throws()
        {
            // Arrange
            var question = new Question(Guid.NewGuid(), ProperText);
            var answer = (Answer)null;

            // Act + Assert
            Assert
                .That(
                    () => Transition.Create(answer, question),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("Answer can't be null!"));
        }

        [Test]
        public void Create_FromAnswerToEnd_EmptyEnd_Throws()
        {
            // Arrange
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var end = (End)null;

            // Act + Assert
            Assert
                .That(
                    () => Transition.Create(answer, end),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("End can't be null!"));
        }

        [Test]
        public void Create_FromAnswerToEnd_EmptyAnswer_Throws()
        {
            // Arrange
            var answer = (Answer)null;
            var end = new End(Guid.NewGuid(), ProperText);

            // Act + Assert
            Assert
                .That(
                    () => Transition.Create(answer, end),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("Answer can't be null!"));
        }

        [Test]
        public void Create_FromQuestinToAnswer_AllConditionsMet_CreateNewTransition()
        {
            // Arrange
            var question = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);

            // Act
            var transition = Transition.Create(question, answer);

            // Assert
            Assert.That(transition.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(transition.FromId, Is.EqualTo(question.Id));
            Assert.That(transition.ToId, Is.EqualTo(answer.Id));
        }

        [Test]
        public void Create_FromAnswerToQuestion_AllConditionsMet_CreateNewTransition()
        {
            // Arrange
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var question = new Question(Guid.NewGuid(), ProperText);            

            // Act
            var transition = Transition.Create(answer, question);

            // Assert
            Assert.That(transition.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(transition.FromId, Is.EqualTo(answer.Id));
            Assert.That(transition.ToId, Is.EqualTo(question.Id));
        }

        [Test]
        public void Create_FromAnswerToEnd_AllConditionsMet_CreateNewTransition()
        {
            // Arrange
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            // Act
            var transition = Transition.Create(answer, end);

            // Assert
            Assert.That(transition.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(transition.FromId, Is.EqualTo(answer.Id));
            Assert.That(transition.ToId, Is.EqualTo(end.Id));
        }
    }
}
