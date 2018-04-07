using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.UnitTests
{
    [TestClass]
    public class MultipleChoiceQuestionTests
    {
        public static int FullPointScore { get; set; } = 10;
        public MultipleChoiceQuestion Question { get; set; } = new MultipleChoiceQuestion()
        {
            Options = new List<string>() { "Option1", "Option2", "Option3" },
            CorrectAnswerIndex = 0,
            Scoring = new Scoring(ScoringMode.FullAnswer, FullPointScore),
            Task = new TaskContent("<p1>ddssd</p1>"),

        };

        [TestMethod]
        public void CheckingAnswer_FullyGoodAnswer()
        {
            var Answer = new MultipleChoiceAnswer()
            {
                SelectedAnswerIndex = 0,
            };
            var points = Question.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore, points);
        }

        [TestMethod]
        public void CheckingAnswer_FullyWrongAnswer()
        {
            var Answer = new MultipleChoiceAnswer()
            {
                SelectedAnswerIndex = 1,
            };

            var points = Question.CheckAnswer(Answer);

            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void CheckingAnswer_WrongAnswer_WrongAnswerIndex()
        {
            var Answer = new MultipleChoiceAnswer()
            {
                SelectedAnswerIndex = 9991,
            };

            var points = Question.CheckAnswer(Answer);

            Assert.AreEqual(0, points);
        }
    }
}
