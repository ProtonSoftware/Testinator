using System;

namespace Testinator.Core.QuestionBuilders
{
    /// <summary>
    /// Builder for any question
    /// </summary>
    /// <typeparam name="TQuestionType">The type of question this builder is going to create</typeparam>
    /// <typeparam name="TOptions">The type of options this question accepts</typeparam>
    /// <typeparam name="TCorrectAnswer">The type of correct answer this question accepts</typeparam>
    public abstract class QuestionBuilder <TQuestionType, TOptions, TCorrectAnswer> : Builder<TQuestionType>
        where TQuestionType : QuestionXXX, new()
    {
        #region Protected Members

        /// <summary>
        /// Indicates if there is a correct answer attached to this question
        /// </summary>
        protected bool IsCorrectAnswerAttached { get; set; }

        /// <summary>
        /// Indicates if there are options attached to this question
        /// </summary>
        protected bool AreOptionsAttached { get; set; }

        /// <summary>
        /// Indicates if there are evaluation criteria attached to this question
        /// </summary>
        protected bool IsScoringAttached { get; set; }

        /// <summary>
        /// Indicates if there is task attached to this question
        /// </summary>
        protected bool IsTaskAttached { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks is the question is ready to be returned
        /// </summary>
        /// <returns>True if it is complete and valid, otherwise false</returns>
        protected override bool IsReady()
        {
            if (IsEverythingIncluded())
            {
                // Call this method only if the question has all the components
                if (IsValid())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks is the question has all elements attached
        /// </summary>
        /// <returns>True if the question is complete, otherwise false</returns>
        private bool IsEverythingIncluded()
            // If it has everything attached it can be returned as a result of this builder's work
            => (IsCorrectAnswerAttached && AreOptionsAttached && IsScoringAttached && IsTaskAttached);
        
        /// <summary>
        /// Loads prototype to this builder
        /// Exceptions: <see cref="NullReferenceException"/> if Prototype is null
        /// </summary>
        /// <param name="Prototype">The prototype of a question for this builder to work with</param>
        private void LoadPrototype(TQuestionType Prototype)
        {
            CreatedObject = Prototype ?? throw new NullReferenceException("Prototype cannot be null");

            // Load flags 
            IsCorrectAnswerAttached = HasCorrectAnswer();
            AreOptionsAttached = HasOptions();
            IsScoringAttached = HasScoring();
            IsTaskAttached = HasTask();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public QuestionBuilder()
        {
            CreatedObject = new TQuestionType();
        }

        /// <summary>
        /// Constructor accepting the model question to work with
        /// Works as an editor in this context
        /// </summary>
        /// <param name="Prototype">The prototype question</param>
        public QuestionBuilder(TQuestionType Prototype)
        {
            LoadPrototype(Prototype);
        }

        #endregion

        #region Public Common Methods

        /// <summary>
        /// Adds a task to the question
        /// </summary>
        /// <param name="Task">The task itself</param>
        public virtual void AddTask(TaskContent Task)
        {
            CreatedObject.Task = Task ?? throw new NullReferenceException();
            IsTaskAttached = true; 
        }

        #endregion

        #region Public Specific Methods

        /// <summary>
        /// Adds options to a question
        /// NOTE: If the question does not support options e.g. SingleTextBoxQuestion, leave the overridden method blank
        /// </summary>
        /// <typeparam name="T">Type of options the question support</typeparam>
        /// <param name="Options">The options for this question</param>
        public abstract void AddOptions(TOptions Options);
        
        /// <summary>
        /// Adds correct answer to the question
        /// </summary>
        /// <typeparam name="T">The type of the correct answer depend on the question type</typeparam>
        /// <param name="CorrectAnswer">The correct answer for the question</param>
        public abstract void AddCorrectAnswer(TCorrectAnswer CorrectAnswer);

        /// <summary>
        /// Adds evaluation criteria to the question
        /// </summary>
        /// <param name="Scoring"></param>
        public abstract void AddScoring(Scoring Scoring);

        #endregion

        #region Protected Common Methods

        /// <summary>
        /// Checks if <see cref="CreatedObject"/> has task attached to it
        /// </summary>
        /// <returns>True if it has; otherwise, false</returns>
        protected virtual bool HasTask() => CreatedObject.Task != null;

        /// <summary>
        /// Checks if <see cref="CreatedObject"/> has scoring attached to it
        /// </summary>
        /// <returns>True if it has; otherwise, false</returns>
        protected virtual bool HasScoring() => CreatedObject.Scoring != null;

        #endregion

        #region Protected Specific Methods

        /// <summary>
        /// Check is the question is valid. Specific to different question types
        /// NOTE: Is called only if all the components of a question are included so that there is no need to check is they are not null
        /// </summary>
        /// <returns>True if all the data is valid, otherwise false</returns>
        protected abstract bool IsValid();

        /// <summary>
        /// Checks if <see cref="CreatedObject"/> has options attached to it
        /// </summary>
        /// <returns>True if it has, otherwise false</returns>
        protected abstract bool HasOptions();

        /// <summary>
        /// Checks if <see cref="CreatedObject"/> has correct answer attached to it
        /// </summary>
        /// <returns>True if it has, otherwise false</returns>
        protected abstract bool HasCorrectAnswer();

        #endregion
    }
}
