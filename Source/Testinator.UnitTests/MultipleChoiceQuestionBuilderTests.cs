using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.UnitTests
{
    /// <summary>
    /// Tutorial https://www.youtube.com/watch?v=HYrXogLj7vg
    /// </summary>
    [TestClass]
    public class MultipleChoiceQuestionBuilderTests
    {
        public MultipleChoiceQuestionXXX CorrectQuestionPrototype { get; set; } = new MultipleChoiceQuestionXXX()
        {
            Task = new TaskContent("test task"),
            Options = new List<string>() { "Option1", "Option2", "Option3" },
            CorrectAnswerIndex = 1,
            Scoring = new Scoring(ScoringMode.FullAnswer, 10),
        };

        public TaskContent Task { get; set; } = new TaskContent("Test task");
        public List<string> Options { get; set; } = new List<string>() { "Option1", "Option2", "Option3" };
        public int CorrectAnswerIndex { get; set; } = 0;
        public Scoring Scoring { get; set; } = new Scoring(ScoringMode.FullAnswer, 21);

        [TestMethod]
        public void CreatingQuestion_FullyValidData()
        {

            var question = AssemblyQuestion();

            Assert.IsTrue(IsCorrect(question));

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingQuestion_NullTask()
        {
            var builder = new MultipleChoiceQuestionBuilder();

            Task = null;

            var question = AssemblyQuestion();
        }

        [TestMethod]
        public void CreatingQuestion_EmptyOptions()
        {
            var builder = new MultipleChoiceQuestionBuilder();

            Options = new List<string>();

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_CorrectAnswerLessThan0()
        {
            var builder = new MultipleChoiceQuestionBuilder();

            CorrectAnswerIndex = -1;

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_CorrectAnswerTooBigWhileAddigIt()
        {
            var builder = new MultipleChoiceQuestionBuilder();

            CorrectAnswerIndex = 888;

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_CorrectAnswerTooBigWhileAddingOptions()
        {
            var builder = new MultipleChoiceQuestionBuilder();

            builder.AddTask(Task);
            builder.AddScoring(Scoring);

            CorrectAnswerIndex = 55;
            builder.AddCorrectAnswer(CorrectAnswerIndex);

            builder.AddOptions(Options);

            var question = builder.GetResult();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_OptionsListTrimming()
        {
            var builder = new MultipleChoiceQuestionBuilder();

            Options[2] = "  kkk   ";

            var question = AssemblyQuestion();

            Assert.IsTrue("kkk" == question.Options[2]);
        }

        [TestMethod]
        public void CreatingQuestion_IdenticalOptions()
        {
            var builder = new MultipleChoiceQuestionBuilder();

            Options[2] = "  kkk   ";
            Options[1] = "kkk";
            Options[0] = "k k k";

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestionFromAnotherQuestion_FullyValidData()
        {
            var builder = new MultipleChoiceQuestionBuilder(CorrectQuestionPrototype);

            var question = builder.GetResult();

            Assert.IsNotNull(question);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Prototype cannot be null")]
        public void CreatingQuestionFromAnotherQuestion_NullPrototype()
        {

            var builder = new MultipleChoiceQuestionBuilder(null);

            var question = builder.GetResult();
        }

        [TestMethod]
        public void CreatingQuestionFromAnotherQuestion_NullOptionsInPrototype()
        {
            CorrectQuestionPrototype.Options = null;

            var builder = new MultipleChoiceQuestionBuilder(CorrectQuestionPrototype);

            var question = builder.GetResult();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestionFromAnotherQuestion_NullTaskInPrototype()
        {
            CorrectQuestionPrototype.Task = null;

            var builder = new MultipleChoiceQuestionBuilder(CorrectQuestionPrototype);

            var question = builder.GetResult();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestionFromAnotherQuestion_NullScoringInPrototype()
        {
            CorrectQuestionPrototype.Scoring = null;

            var builder = new MultipleChoiceQuestionBuilder(CorrectQuestionPrototype);

            var question = builder.GetResult();

            Assert.IsNull(question);
        }

        private MultipleChoiceQuestionXXX AssemblyQuestion()
        {
            var builder = new MultipleChoiceQuestionBuilder();


            builder.AddOptions(Options);
            builder.AddTask(Task);
            builder.AddCorrectAnswer(CorrectAnswerIndex);
            builder.AddScoring(Scoring);

            return builder.GetResult();
        }

        private bool IsCorrect(MultipleChoiceQuestionXXX question)
        {
            var TaskIsCorrect = question.Task == Task;
            var OptionsAreCorrect = question.Options == Options;
            var CorrectAnswerIsCorrect = question.CorrectAnswerIndex == CorrectAnswerIndex;
            var ScoringIsCorrect = question.Scoring == Scoring;

            var ifCorrect = TaskIsCorrect && OptionsAreCorrect && CorrectAnswerIsCorrect && ScoringIsCorrect;

            return ifCorrect;
        }
    }
}
