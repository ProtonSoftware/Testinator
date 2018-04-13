using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Builds a test or edits an existing one
    /// </summary>
    public class TestBuilder : Builder<Test>
    {
        #region Public Memebrs

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
        /// WARNING: while setting this value make sure it is between 0 and <see cref="MaximumTestDurationSeconds"/>
        /// </summary>
        public const int MinimumTestDurationSeconds = 60;

        /// <summary>
        /// Maximum test duration time in seconds
        /// WARNING: while setting this value make sure it is bigger than <see cref="MinimumTestDurationSeconds"/>
        /// </summary>
        public const int MaximumTestDurationSeconds = 10800; // 3 hours

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

        /// <summary>
        /// Maximum length of a note attached to the test
        /// </summary>
        public const int MaximumNoteLength = 300;

        /// <summary>
        /// Maximum number of tags in a test
        /// </summary>
        public const int MaximumTagsNumber = 5;

        #endregion

        #region Public Properties
        
        /// <summary>
        /// Current test info from the test
        /// </summary>
        public TestInformation CurrentTestInfo => CreatedObject.Info;

        /// <summary>
        /// Current questions from this test
        /// </summary>
        public List<Question> CurrentQuestions => CreatedObject.Questions;

        /// <summary>
        /// Current total score of this test
        /// </summary>
        public int CurrentPointScore => CreatedObject.TotalPointScore;

        /// <summary>
        /// The name of currently created/editer test
        /// </summary>
        public string CurrentTestName => CreatedObject != null ? CreatedObject.Info.Name : ""; 

        #endregion

        #region Public Construction Methods

        #region Name

        /// <summary>
        /// Adds the name of this test
        /// </summary>
        /// <param name="NewName">New name</param>
        public void AddName(string NewName)
        {
            if (string.IsNullOrEmpty(NewName))
                throw new NullReferenceException("Name cannot be empty!");

            if (NewName.Length < MinimumTestNameLength || NewName.Length > MaximumTestNameLength)
                throw new Exception($"Nieprawidłowa długość nazwy testu. Limit to: {MinimumTestNameLength}-{MaximumTestNameLength} znaków.");

            CreatedObject.Info.Name = NewName;
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
                // remake this plz
                throw new Exception($"Test nie może tyle trwać! Przedział to {MaximumTestDurationSeconds / 3600}:" +
                    $"{(MaximumTestDurationSeconds % 60)}:" +
                    $"{(MaximumTestDurationSeconds % 60) / 60} - {MinimumTestDurationSeconds / 3600}: " +
                    $"{(MinimumTestDurationSeconds % 60)}:{(MaximumTestDurationSeconds % 60) / 60}.");

            CreatedObject.Info.Duration = NewDuration;
        }

        #endregion

        #region Note

        /// <summary>
        /// Adds note to this test
        /// </summary>
        /// <param name="newNote">New note</param>
        public void AddNote(string newNote)
        {
            if (newNote != null && newNote.Length > MaximumNoteLength)
                throw new Exception($"Nieprawidłowa długość notatki. Limit to: {MaximumNoteLength} znaków.");


            CreatedObject.Info.Note = newNote;
        }

        #endregion

        #region Tags

        /// <summary>
        /// Adds tags to this test
        /// </summary>
        /// <param name="newTags">New tags</param>
        public void AddTags(string newTags)
        {
            // TODO: tags recognising and error checks
            if (newTags != null && newTags.Length > MaximumTestNameLength)
                throw new Exception("Invalid tags number");

            CreatedObject.Info.Tags = newTags;
        }

        #endregion

        #region Version

        /// <summary>
        /// Adds version of software this test was last edited on
        /// </summary>
        /// <param name="newVersion">New value</param>
        public void AddVersion(Version newVersion)
        {
            CreatedObject.Info.SoftwareVersion = newVersion ?? throw new NullReferenceException();
        }

        #endregion

        #region Questions

        /// <summary>
        /// Adds a question to this test
        /// </summary>
        /// <param name="QuestionToAdd">To question to add</param>
        public void AddQuestion(Question QuestionToAdd)
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
        public void RemoveQuestion(Question QuestionToRemove)
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
        public void UpdateQuestion(Question OldQuestion, Question NewQuestion)
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
            return (!string.IsNullOrEmpty(CreatedObject.Info.Name)) &&

                   (CreatedObject.Info.Duration != null && IsDurationInRange(CreatedObject.Info.Duration)) &&

                   (CreatedObject.Questions != null && CreatedObject.Questions.Count >= MinimumQuestionsCount && CreatedObject.Questions.Count <= MaximumQuestionsCount) &&

                   (CreatedObject.Grading != null && CreatedObject.Grading.GetMaxScore() == CreatedObject.TotalPointScore);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Checks is the given duration is bettwen the limits
        /// </summary>
        /// <param name="TimeToCheck">Time span to check if it is in range</param>
        /// <returns>True if it is between <see cref="MinimumTestDurationSeconds"/> and <see cref="MaximumTestDurationSeconds"/>; otherwise, false</returns>
        private bool IsDurationInRange(TimeSpan TimeToCheck)
            => !(TimeToCheck.TotalSeconds < MinimumTestDurationSeconds || TimeToCheck.TotalSeconds > MaximumTestDurationSeconds);
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a barnd new test
        /// </summary>
        public TestBuilder()
        {
            CreatedObject = new Test();
        }

        /// <summary>
        /// Works on a given prototype object
        /// </summary>
        /// <param name="Prototype"></param>
        public TestBuilder(Test Prototype)
        {
            CreatedObject = Prototype;
        }

        #endregion
    }
}
