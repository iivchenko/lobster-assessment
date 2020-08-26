using NUnit.Framework;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Linq;

namespace Questionnaire.Application.Tests.Domain
{
    [TestFixture]
    public sealed class PollTests
    {
        private const string ProperName = "Some Name";
        private const string ProperDescription = "Some Description";
        private const string ProperText = "Some Text";

        [Test]
        public void Create_EmptyId_Throws()
        {
            //Arrange+Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.Empty, null, null, null, null, null),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'id' can't be empty!"));
        }

        [Test]
        public void Create_NameIsEmpty_Throws()
        {
            //Arrange+Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), string.Empty, null, null, null, null),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'name' can't be empty!"));
        }

        [Test]
        public void Create_DescriptionIsEmpty_Throws()
        {
            //Arrange+Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, string.Empty, null, null, null),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'description' can't be empty!"));
        }

        [Test]
        public void Create_EmptyRootQuestionId_Throws()
        {
            //Arrange+Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, ProperDescription, null, null, null),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'rootQuestion' can't be empty!"));
        }

        [Test]
        public void Create_EmptyItems_Throws()
        {
            // Arrange
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);

            // Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, Enumerable.Empty<PollItem>(), null),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'items' can't be empty!"));
        }

        [Test]
        public void Create_EmptyTransitions_Throws()
        {
            // Arrange 
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var items = new PollItem [] { rootQuestion, new End(Guid.NewGuid(), ProperText) };

            // Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, Enumerable.Empty<Transition>()),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo("'transitions' can't be empty!"));
        }

        [Test]
        public void Create_RootQuestionIdNotPresentInItems_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var question = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);            
            var items = new PollItem [] { question, answer };
            var transitions = new[] { Transition.Create(answer, question) };

            // Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Root questioin (id: '{rootQuestion.Id}' is not presetn in items!"));
        }

        [Test]
        public void Create_RootQuestionNotPresentInTransitionis_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var question = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var items = new PollItem[] { rootQuestion, question, answer };
            var transitions = new[] { Transition.Create(answer, question) };

            // Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Question (id: '{rootQuestion.Id}' should have transition!"));
        }

        [Test]
        public void Create_QuestionNotPresentInTransitions_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var question = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var items = new PollItem[] { rootQuestion, question, answer };
            var transitions = new[] { Transition.Create(rootQuestion, answer) };

            // Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Question (id: '{question.Id}' should have transition!"));
        }

        [Test]
        public void Create_AnswerNotPresentInTransitions_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var items = new PollItem[] { rootQuestion, answer };
            var transitions = new[] { Transition.Create(rootQuestion, answer) };

            // Act+Assert
            Assert
                .That(
                    () => new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Answer (id: '{answer.Id}' should have transition!"));
        }

        [Test]
        public void FindNextFor_Question_QuestionIsNull_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            var items = new PollItem[] { rootQuestion, answer, end };
            var transitions = new[] { Transition.Create(rootQuestion, answer), Transition.Create(answer, end) };

            var poll = new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions);

            // Act+Assert
            Assert
                .That(
                    () => poll.FindNextFor((Question)null),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Question should not be null!"));
        }

        [Test]
        public void FindNextFor_Question_QuestionIsNotPresentInPoll_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            var fakeQuestion = new Question(Guid.NewGuid(), ProperText);

            var items = new PollItem[] { rootQuestion, answer, end };
            var transitions = new[] { Transition.Create(rootQuestion, answer), Transition.Create(answer, end) };

            var poll = new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions);

            // Act+Assert
            Assert
                .That(
                    () => poll.FindNextFor(fakeQuestion),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Question (id: '{fakeQuestion.Id}' not present in the Poll!"));
        }

        [Test]
        public void FindNextFor_Question_AllConditionsAreMet_ReturnAnswers()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer1 = new Answer(Guid.NewGuid(), ProperText);
            var answer2 = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            var items = new PollItem[] { rootQuestion, answer1, answer2, end };
            var transitions = new[] 
                { 
                    Transition.Create(rootQuestion, answer1),
                    Transition.Create(rootQuestion, answer2),
                    Transition.Create(answer1, end),
                    Transition.Create(answer2, end)
                };

            var poll = new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions);

            // Act
            var actualAnswers = poll.FindNextFor(rootQuestion);

            // Assert
            Assert.That(actualAnswers.Count(), Is.EqualTo(2));
            Assert.That(actualAnswers, Contains.Item(answer1));
            Assert.That(actualAnswers, Contains.Item(answer2));
        }

        [Test]
        public void FindNextFor_Answer_AnsswerIsNull_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            var items = new PollItem[] { rootQuestion, answer, end };
            var transitions = new[] { Transition.Create(rootQuestion, answer), Transition.Create(answer, end) };

            var poll = new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions);

            // Act+Assert
            Assert
                .That(
                    () => poll.FindNextFor((Answer)null),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Answer should not be null!"));
        }

        [Test]
        public void FindNextFor_Answer_AnswerIsNotPresentInPoll_Throws()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            var fakeAnswer = new Answer(Guid.NewGuid(), ProperText);

            var items = new PollItem[] { rootQuestion, answer, end };
            var transitions = new[] { Transition.Create(rootQuestion, answer), Transition.Create(answer, end) };

            var poll = new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions);

            // Act+Assert
            Assert
                .That(
                    () => poll.FindNextFor(fakeAnswer),
                    Throws
                        .InstanceOf<DomainException>()
                        .With
                        .Message
                        .EqualTo($"Answer (id: '{fakeAnswer.Id}' not present in the Poll!"));
        }

        [Test]
        public void FindNextFor_Answer_AllConditionsAreMet_ReturnEnd()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            var items = new PollItem[] { rootQuestion, answer, end };
            var transitions = new[]
                {
                    Transition.Create(rootQuestion, answer),
                    Transition.Create(answer, end)
                };

            var poll = new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions);

            // Act
            var actualEnd = poll.FindNextFor(answer);

            // Assert
            Assert.That(actualEnd, Is.EqualTo(end));
        }

        [Test]
        public void FindNextFor_Answer_AllConditionsAreMet_ReturnQuestion()
        {
            // Arrange           
            var rootQuestion = new Question(Guid.NewGuid(), ProperText);
            var answer1 = new Answer(Guid.NewGuid(), ProperText);
            var question = new Question(Guid.NewGuid(), ProperText);
            var answer2 = new Answer(Guid.NewGuid(), ProperText);
            var end = new End(Guid.NewGuid(), ProperText);

            var items = new PollItem[] { rootQuestion, answer1, question, answer2, end };
            var transitions = new[]
                {
                    Transition.Create(rootQuestion, answer1),
                    Transition.Create(answer1, question),
                    Transition.Create(question, answer2),
                    Transition.Create(answer2, end)
                };

            var poll = new Poll(Guid.NewGuid(), ProperName, ProperDescription, rootQuestion, items, transitions);

            // Act
            var actualQuestion = poll.FindNextFor(answer1);

            // Assert
            Assert.That(actualQuestion, Is.EqualTo(question));
        }
    }
}
