using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.UnitTests
{
    [TestClass]
    public class TestBuilderTests
    {
        public static MultipleChoiceQuestionXXX AddedQuestion = new MultipleChoiceQuestionXXX()
        {
            CorrectAnswerIndex = 0,
            Options = new List<string>() { "option1", "option2" },
            Scoring = new Scoring(ScoringMode.FullAnswer, 10),
            Task = new TaskContent("Task6"),
        };

        public static TestXXX CorrectTest = new TestXXX()
        {
            Duration = TimeSpan.FromSeconds(TestBuilder.MinimumTestDurationSeconds), 
            Questions = new List<QuestionXXX>()
            {
                new MultipleChoiceQuestionXXX()
                {
                    CorrectAnswerIndex = 0,
                    Options = new List<string>() {"option1", "option2"},
                    Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                    Task = new TaskContent("Task1"),
                },
                new MultipleChoiceQuestionXXX()
                {
                    CorrectAnswerIndex = 0,
                    Options = new List<string>() {"option1", "option2"},
                    Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                    Task = new TaskContent("Task2"),
                },
                new MultipleChoiceQuestionXXX()
                {
                    CorrectAnswerIndex = 0,
                    Options = new List<string>() {"option1", "option2"},
                    Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                    Task = new TaskContent("Task3"),
                },
                new MultipleChoiceQuestionXXX()
                {
                    CorrectAnswerIndex = 0,
                    Options = new List<string>() {"option1", "option2"},
                    Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                    Task = new TaskContent("Task4"),
                },
                new MultipleChoiceQuestionXXX()
                {
                    CorrectAnswerIndex = 0,
                    Options = new List<string>() {"option1", "option2"},
                    Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                    Task = new TaskContent("Task5"),
                },
                AddedQuestion,
            },
            TotalPointScore = 60,
            Name = "some name 1",

        };


        #region Name

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTestFromScratch_AddingName_NullValue()
        {
            var Builder = new TestBuilder();
            Builder.AddName(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTestFromScratch_AddingName_EmptyString()
        {
            var Builder = new TestBuilder();
            Builder.AddName("");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingName_TooShortName()
        {
            var Builder = new TestBuilder();
            var name = new string('c', TestBuilder.MinimumTestNameLength - 1);
            Builder.AddName(name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingName_TooLongName()
        {
            var Builder = new TestBuilder();
            var name = new string('c', TestBuilder.MaximumTestNameLength + 1);
            Builder.AddName(name);
        }

        [TestMethod]
        public void CreatingTestFromScratch_AddingName_CorrectValue()
        {
            var Builder = new TestBuilder();
            var name = new string('c', TestBuilder.MinimumTestNameLength);
            Builder.AddName(name);
            
        }

        #endregion

        #region Duration

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingDuration_EmptyValue()
        {
            var Builder = new TestBuilder();
            Builder.AddDuration(new TimeSpan());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingDuration_TooLongValue()
        {
            var Builder = new TestBuilder();
            Builder.AddDuration(TimeSpan.FromSeconds(TestBuilder.MaximumTestDurationSecodns + 1));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingDuration_TooSmallValue()
        {
            var Builder = new TestBuilder();
            Builder.AddDuration(TimeSpan.FromSeconds(TestBuilder.MinimumTestDurationSeconds - 1));
        }

        [TestMethod]
        public void CreatingTestFromScratch_AddingDuration_GoodValue()
        { 
            var Builder = new TestBuilder();
            Builder.AddDuration(TimeSpan.FromSeconds(TestBuilder.MinimumTestDurationSeconds));
        }

        #endregion

        #region Grading

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTestFromScratch_AddingGrading_NullValue()
        {
            var Builder = new TestBuilder();
            Builder.AddGrading(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingGrading_TooSmallScore()
        {
            var Builder = new TestBuilder();
            var grading = new GradingPoints();
            grading.UpdateMark(Marks.A, 100, 80);

            var Question = new MultipleChoiceQuestionXXX()
            {
                CorrectAnswerIndex = 1,
                Options = new List<string>() { "1", "2", "3" },
                Task = new TaskContent("dsdsd"),
                Scoring = new Scoring(ScoringMode.FullAnswer, 150),
            };

            Builder.AddQuestion(Question);

            Builder.AddGrading(grading);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingGrading_TooBigScore()
        {
            var Builder = new TestBuilder();
            var grading = new GradingPoints();
            grading.UpdateMark(Marks.A, 100, 80);

            var Question = new MultipleChoiceQuestionXXX()
            {
                CorrectAnswerIndex = 1,
                Options = new List<string>() { "1", "2", "3" },
                Task = new TaskContent("dsdsd"),
                Scoring = new Scoring(ScoringMode.FullAnswer, 50),
            };

            Builder.AddQuestion(Question);

            Builder.AddGrading(grading);
        }


        [TestMethod]
        public void CreatingTestFromScratch_AddingGrading_CorrectScore()
        {
            var Builder = new TestBuilder();
            var grading = new GradingPoints();
            grading.UpdateMark(Marks.A, 100, 80);

            var Question = new MultipleChoiceQuestionXXX()
            {
                CorrectAnswerIndex = 1,
                Options = new List<string>() { "1", "2", "3" },
                Task = new TaskContent("dsdsd"),
                Scoring = new Scoring(ScoringMode.FullAnswer, 100),
            };

            Builder.AddQuestion(Question);

            Builder.AddGrading(grading);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTestFromScratch_AddingQuestion_NullQuestion()
        {
            var Builder = new TestBuilder();

            Builder.AddQuestion(null);
            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingQuestion_MaxQuestionsNumberReached()
        {

            var Test = new TestXXX()
            {
                Questions = new List<QuestionXXX>(new QuestionXXX[TestBuilder.MaximumQuestionsCount]),
            };

            var Builder = new TestBuilder(Test);

            Builder.AddQuestion(new MultipleChoiceQuestionXXX());

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreatingTestFromScratch_AddingQuestion_AddingThisSameQuestion()
        {

            var Builder = new TestBuilder();
            var q = new MultipleChoiceQuestionXXX()
            {
                Scoring = new Scoring(ScoringMode.EvenParts, 10),
            };
            Builder.AddQuestion(q);
            Builder.AddQuestion(q);

        }

        [TestMethod]
        public void CreatingTestFromScratch_AddingQuestion_CorrectQuestion()
        {

            var Builder = new TestBuilder();
            var q = new MultipleChoiceQuestionXXX()
            {
                Scoring = new Scoring(ScoringMode.EvenParts, 10),
            };
            Builder.AddQuestion(q);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTestFromScratch_RemovingQuestion_NullValue()
        {
            var Builder = new TestBuilder();

            Builder.RemoveQuestion(null);
        }

        [TestMethod]
        public void CreatingTestFromScratch_RemovingQuestion_GoodValue()
        {
            var Builder = new TestBuilder(CorrectTest);
            
            Builder.RemoveQuestion(AddedQuestion);


            var CorrectGrading = new GradingPoints();
            CorrectGrading.UpdateMark(Marks.A, 50, 50);
            Builder.AddGrading(CorrectGrading);

            var Test = Builder.GetResult();

            Assert.IsTrue(!Test.Questions.Contains(AddedQuestion));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTestFromScratch_UpdatingQuestion_NullValues()
        {
            var Builder = new TestBuilder(CorrectTest);

            Builder.UpdateQuestion(null, null);
        }

        [TestMethod]
        public void CreatingTestFromScratch_UpdatingQuestion_GoodValue()
        {
            var Builder = new TestBuilder(CorrectTest);

            var NewQuestion = new SingleTextBoxQuestionXXX()
            {
                CorrectAnswer = "ddd",
                Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                Task = new TaskContent("dssd"),
            };

            Builder.UpdateQuestion(AddedQuestion, NewQuestion);
            var CorrectGrading = new GradingPoints();

            CorrectGrading.UpdateMark(Marks.A, 60, 50);
            Builder.AddGrading(CorrectGrading);

            var test = Builder.GetResult();

            Assert.IsTrue(test.Questions.Contains(NewQuestion));
        }

        #endregion

    }
}
