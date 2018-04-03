using System;

namespace Testinator.Core
{
    /// <summary>
    /// Builds a test or edits an existing one
    /// </summary>
    public class TestBuilder : Builder<TestXXX>
    {
        #region Public Properties

        /// <summary>
        /// The minimum numebr of questions that a sigle test can have
        /// WARNING: while setting this value make sure it is between 0 and <see cref="MaximumQuestionsCount"/>
        /// </summary>
        public const int MinimumQuestionsCount = 1;

        /// <summary>
        /// Maximum number of question that a single test can have
        /// WARNING: while setting this value make sure it is bigger than <see cref="MinimumQuestionsCount"/>
        /// </summary>
        public const int MaximumQuestionsCount = 50;

        /// <summary>
        /// Miminum test duration time in seconds
        /// WARNING: while setting this value make sure it is between 0 and <see cref="MaximumTestDurationSecodns"/>
        /// </summary>
        public const int MinimumTestDurationSeconds = 60;

        /// <summary>
        /// Maximum test duration time in seconds
        /// WARNING: while setting this value make sure it is bigger than <see cref="MinimumTestDurationSeconds"/>
        /// </summary>
        public const int MaximumTestDurationSecodns = 10800; // 3 hours

        /// <summary>
        /// Maximum number of character that a test name can have
        /// WARNING: while setting this value make sure it is bigger than <see cref="MinimumTestNameLength"/>
        /// </summary>
        public const int MaximumTestNameLength = 250;

        /// <summary>
        /// Mimnum number of character that a test name can have
        /// WARNING: while setting this value make sure it is between 0 and <see cref="MaximumTestNameLength"/>
        /// </summary>
        public const int MinimumTestNameLength = 5;

        #endregion

        #region Public Construction Methods

        #region Name

        /// <summary>
        /// Adds the name of this test
        /// </summary>
        /// <param name="Name">New name</param>
        public void AddName(string NewName)
        {
            if (string.IsNullOrEmpty(NewName))
                throw new NullReferenceException("Name cannot be empty!");

            if (NewName.Length < MinimumTestNameLength || NewName.Length > MaximumTestNameLength)
                throw new Exception("Invalid name length");

            CreatedObject.Name = NewName;
        }

        #endregion

        #region Duration

        /// <summary>
        /// Adds a new duration of this test
        /// </summary>
        /// <param name="NewDuration">The new duration of this test</param>
        public void AddDuration(TimeSpan NewDuration)
        {
            if (NewDuration == null)
                throw new NullReferenceException("Duration cannot be null");

            if (!IsDurationInRange(NewDuration))
                throw new Exception("Invalid test duration");

            CreatedObject.Duration = NewDuration;
        }

        #endregion

        #region Questions

        /// <summary>
        /// Adds a question to this test
        /// </summary>
        /// <param name="QuestionToAdd">To question to add</param>
        public void AddQuestion(QuestionXXX QuestionToAdd)
        {
            if (QuestionToAdd == null)
                throw new NullReferenceException("Value cannot be null");

            if (CreatedObject.Questions.Count >= MaximumQuestionsCount)
                throw new Exception("Cannot add more questions to this test");

            // Do not at this same question
            // NOTE: can be changed
            if (CreatedObject.Questions.Contains(QuestionToAdd))
                throw new Exception("This question has already been added to this test");

            CreatedObject.Questions.Add(QuestionToAdd);

            // Update maximum point score
            CreatedObject.TotalPointScore += QuestionToAdd.Scoring.FullPointScore;
        }

        /// <summary>
        /// Removes a question from this test
        /// </summary>
        /// <param name="QuestionToRemove">Question to remove</param>
        public void RemoveQuestion(QuestionXXX QuestionToRemove)
        {
            if (QuestionToRemove == null)
                throw new NullReferenceException("Value cannot be null");

            // Make sure the test contains this question
            // NOTE: can be changed
            if (!CreatedObject.Questions.Contains(QuestionToRemove))
                return;

            CreatedObject.Questions.Remove(QuestionToRemove);

            // Update maximum point score
            CreatedObject.TotalPointScore -= QuestionToRemove.Scoring.FullPointScore;
        }


        /// <summary>
        /// Updates a question 
        /// </summary>
        /// <param name="OldQuestion">The old question to be replaced/updated</param>
        /// <param name="NewQuestion">New question to be put in the old one's place</param>
        public void UpdateQuestion(QuestionXXX OldQuestion, QuestionXXX NewQuestion)
        {
            if (OldQuestion == null || NewQuestion == null)
                throw new NullReferenceException("Value cannot be null");

            // Make sure the test contains the old question
            // NOTE: can be changed
            if (!CreatedObject.Questions.Contains(OldQuestion))
                return;

            // Keep the old index
            var OldQuestionsIndex = CreatedObject.Questions.IndexOf(OldQuestion);

            CreatedObject.Questions.Remove(OldQuestion);

            // Keep total point score up to date so there is no need to caltulate it every time
            CreatedObject.TotalPointScore -= OldQuestion.Scoring.FullPointScore;

            // Insert at the old question's position to maintain the question order
            CreatedObject.Questions.Insert(OldQuestionsIndex, NewQuestion);

            CreatedObject.TotalPointScore += NewQuestion.Scoring.FullPointScore;
        }

        #endregion

        #region Grading 

        /// <summary>
        /// Adds grading to the test
        /// </summary>
        /// <param name="NewGrading">New grading to be attached</param>
        public void AddGrading(GradingPoints NewGrading)
        {
            if (NewGrading == null)
                throw new NullReferenceException("Value cannot be null");

            if (NewGrading.GetMaxScore() != CreatedObject.TotalPointScore)
                throw new Exception("This grading does not match this test");

            CreatedObject.Grading = NewGrading;
        }

        #endregion

        #endregion

        #region Private Overridden Methods

        /// <summary>
        /// Checks if the test is ready to be returned
        /// </summary>
        /// <returns>True if the test is ready; otherwise, false</returns>
        protected override bool IsReady()
        {
            return (!string.IsNullOrEmpty(CreatedObject.Name)) &&

                   (CreatedObject.Duration != null && IsDurationInRange(CreatedObject.Duration)) &&

                   (CreatedObject.Questions != null && CreatedObject.Questions.Count >= MinimumQuestionsCount && CreatedObject.Questions.Count <= MaximumQuestionsCount) &&

                   (CreatedObject.Grading != null && CreatedObject.Grading.GetMaxScore() == CreatedObject.TotalPointScore);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Checks is the given duration is bettwen the limits
        /// </summary>
        /// <param name="TimeToCheck">Time span to check if it is in range</param>
        /// <returns>True if it is between <see cref="MinimumTestDurationSeconds"/> and <see cref="MaximumTestDurationSecodns"/>; otherwise, false</returns>
        private bool IsDurationInRange(TimeSpan TimeToCheck)
            => !(TimeToCheck.TotalSeconds < MinimumTestDurationSeconds || TimeToCheck.TotalSeconds > MaximumTestDurationSecodns);
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a barnd new test
        /// </summary>
        public TestBuilder()
        {
            CreatedObject = new TestXXX();
        }

        /// <summary>
        /// Works on a given prototype object
        /// </summary>
        /// <param name="Prototype"></param>
        public TestBuilder(TestXXX Prototype)
        {
            CreatedObject = Prototype;
        }

        #endregion
    }
}
