using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.UnitTests
{
    [TestClass]
    public class MultipleCheckBoxesQuestionTests
    {
        public static int FullPointScore { get; set; } = 2;
        public static List<bool> CorrectAnswer_3Options { get; set; } = new List<bool>() { true, false, true };
        public MultipleCheckBoxesQuestionXXX Question_3Options { get; set; } = new MultipleCheckBoxesQuestionXXX()
        {
            Options = new List<string>() { "Option1", "Option2", "Option3" },
            CorrectAnswer  = CorrectAnswer_3Options,
            Scoring = new Scoring(ScoringMode.FullAnswer, FullPointScore),
            Task = new TaskContent("<p1>ddssd</p1>"),

        };

        public static List<bool> CorrectAnswer_4Options_4Correct { get; set; } = new List<bool>() { true, true, true, true };
        public static List<bool> CorrectAnswer_4Options_3Correct { get; set; } = new List<bool>() { false, true, true, true };
        public static List<bool> CorrectAnswer_4Options_2Correct { get; set; } = new List<bool>() { false, false, true, true };
        public static List<bool> CorrectAnswer_4Options_1Correct { get; set; } = new List<bool>() { false, false, false, true };
        public static List<bool> CorrectAnswer_4Options_0Correct { get; set; } = new List<bool>() { false, false, false, false };

        public MultipleCheckBoxesQuestionXXX Question_4Options_HalfAnswerMode { get; set; } = new MultipleCheckBoxesQuestionXXX()
        {
            Options = new List<string>() { "Option1", "Option2", "Option3", "Option4" },
            CorrectAnswer = CorrectAnswer_4Options_4Correct,
            Scoring = new Scoring(ScoringMode.HalfTheAnswer, FullPointScore),
            Task = new TaskContent("<p1>ddssd</p1>"),

        };

        public MultipleCheckBoxesQuestionXXX Question_4Options_EvenPartsMode { get; set; } = new MultipleCheckBoxesQuestionXXX()
        {
            Options = new List<string>() { "Option1", "Option2", "Option3", "Option4" },
            CorrectAnswer = CorrectAnswer_4Options_4Correct,
            Scoring = new Scoring(ScoringMode.EvenParts, FullPointScore),
            Task = new TaskContent("<p1>ddssd</p1>"),

        };

        [TestMethod]
        public void CheckingAnswer_FullScoreMode_FullyGoodAnswer()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_3Options,
            };

            var points = Question_3Options.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore, points);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CheckingAnswer_NullAnswerContent()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = null,
            };

            var points = Question_3Options.CheckAnswer(Answer);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CheckingAnswer_NullAnswer()
        {
            var points = Question_3Options.CheckAnswer(null);
        
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckingAnswer_NotMatchingAnswerCount()
        {
            var answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = new List<bool>(CorrectAnswer_3Options.Count + 1),
            };

            var points = Question_3Options.CheckAnswer(answer);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckingAnswer_NotMatchingAnswerType()
        {
            var answer = new SingleTextBoxAnswerXXX()
            {
               UserAnswer = "",
            };

            var points = Question_3Options.CheckAnswer(answer);

        }

        [TestMethod]
        public void CheckingAnswer_FullScoreMode_FullyWrong()
        {
            var badAnswer = new List<bool>();
            foreach (var item in CorrectAnswer_3Options)
                badAnswer.Add(!item);

            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = badAnswer,
            };

            var points = Question_3Options.CheckAnswer(Answer);

            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void CheckingAnswer_HalfTheAnswerMode_4OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_4Correct,
            };


            var points = Question_4Options_HalfAnswerMode.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore, points);
        }

        [TestMethod]
        public void CheckingAnswer_HalfTheAnswerMode_3OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_3Correct,
            };
            

            var points = Question_4Options_HalfAnswerMode.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore / 2, points);
        }

        [TestMethod]
        public void CheckingAnswer_HalfTheAnswerMode_2OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_2Correct,
            };


            var points = Question_4Options_HalfAnswerMode.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore / 2, points);
        }

        [TestMethod]
        public void CheckingAnswer_HalfTheAnswerMode_1OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_1Correct,
            };


            var points = Question_4Options_HalfAnswerMode.CheckAnswer(Answer);

            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void CheckingAnswer_HalfTheAnswerMode_0OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_0Correct,
            };


            var points = Question_4Options_HalfAnswerMode.CheckAnswer(Answer);

            Assert.AreEqual(0, points);
        }


        [TestMethod]
        public void CheckingAnswer_EvenPartsMode_0OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_0Correct,
            };


            var points = Question_4Options_EvenPartsMode.CheckAnswer(Answer);

            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void CheckingAnswer_EvenPartsMode_1OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_1Correct,
            };


            var points = Question_4Options_EvenPartsMode.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore / 4, points);
        }

        [TestMethod]
        public void CheckingAnswer_EvenPartsMode_2OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_2Correct,
            };


            var points = Question_4Options_EvenPartsMode.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore * 2 / 4, points);
        }

        [TestMethod]
        public void CheckingAnswer_EvenPartsMode_3OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_3Correct,
            };


            var points = Question_4Options_EvenPartsMode.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore * 3 / 4, points);
        }


        [TestMethod]
        public void CheckingAnswer_EvenPartsMode_4OutOf4correct()
        {
            var Answer = new MultipleCheckBoxesAnswerXXX()
            {
                UserAnswer = CorrectAnswer_4Options_4Correct,
            };


            var points = Question_4Options_EvenPartsMode.CheckAnswer(Answer);

            Assert.AreEqual(FullPointScore, points);
        }
    }
}
