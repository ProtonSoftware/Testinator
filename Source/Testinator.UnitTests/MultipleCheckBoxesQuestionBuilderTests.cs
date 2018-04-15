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
    public class MultipleCheckBoxesQuestionBuilderTests
    {
        public MultipleCheckBoxesQuestion CorrectQuestionPrototype { get; set; } = new MultipleCheckBoxesQuestion()
        {
            Task = new TaskContent("<p1>Test</p1>"),
            Options = new List<string>() { "Option1", "Option2", "Option3" },
            CorrectAnswer = new List<bool>() { true, true, false},
            Scoring = new Scoring(ScoringMode.FullAnswer, 10),
        };

        public TaskContent Task { get; set; } = new TaskContent("<p1>HtmlQuestionTask</p1>");
        public List<string> Options { get; set; } = new List<string>() { "Option1", "Option2", "Option3" };
        public List<bool> CorrectAnswer { get; set; } = new List<bool>() { true, true, false };
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
            var builder = new MultipleCheckBoxesQuestionBuilder();

            Task = null;

            var question = AssemblyQuestion();

        }

        [TestMethod]
        public void CreatingQuestion_NullOptions()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();

            Options = null;

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_EmptyOptions()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();

            Options = new List<string>();

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }
        
        [TestMethod]
        public void CreatingQuestion_NullAnswers()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();

            CorrectAnswer = null;

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_TooBigAnswerCount()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();

            CorrectAnswer = new List<bool>() { true, false, false, false};

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_TooSmallAnswerCount()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();

            CorrectAnswer = new List<bool>() { true,};

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestion_OptionsListTrimming()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();

            Options[2] = "  kkk   ";

            var question = AssemblyQuestion();

            Assert.IsTrue("kkk" == question.Options[2]);
        }

        [TestMethod]
        public void CreatingQuestion_IdenticalOptions()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();

            Options[2] = "  kkk   ";
            Options[1] = "kkk";
            Options[0] = "k k k";

            var question = AssemblyQuestion();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestionFromAnotherQuestion_FullyValidData()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder(CorrectQuestionPrototype);

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

            var builder = new MultipleCheckBoxesQuestionBuilder(CorrectQuestionPrototype);

            var question = builder.GetResult();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestionFromAnotherQuestion_NullTaskInPrototype()
        {
            CorrectQuestionPrototype.Task = null;

            var builder = new MultipleCheckBoxesQuestionBuilder(CorrectQuestionPrototype);

            var question = builder.GetResult();

            Assert.IsNull(question);
        }

        [TestMethod]
        public void CreatingQuestionFromAnotherQuestion_NullScoringInPrototype()
        {
            CorrectQuestionPrototype.Scoring = null;

            var builder = new MultipleCheckBoxesQuestionBuilder(CorrectQuestionPrototype);

            var question = builder.GetResult();

            Assert.IsNull(question);
        }

        private MultipleCheckBoxesQuestion AssemblyQuestion()
        {
            var builder = new MultipleCheckBoxesQuestionBuilder();


            builder.AddOptions(Options);
            builder.AddTask(Task);
            builder.AddCorrectAnswer(CorrectAnswer);
            builder.AddScoring(Scoring);

            return builder.GetResult();
        }
        
        private bool IsCorrect(MultipleCheckBoxesQuestion question)
        {
            var TaskIsCorrect = question.Task == Task;
            var OptionsAreCorrect = question.Options == Options;
            var CorrectAnswerIsCorrect = question.CorrectAnswer == CorrectAnswer;
            var ScoringIsCorrect = question.Scoring == Scoring;

            var ifCorrect = TaskIsCorrect && OptionsAreCorrect && CorrectAnswerIsCorrect && ScoringIsCorrect;

            return ifCorrect;
        }
    }
}
