using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Base view model for question editors
    /// </summary>
    public abstract class BaseQuestionEditorViewModel: BaseViewModel
    {
        #region Private Memebrs

        /// <summary>
        /// Common properties that can raise a question changed event
        /// </summary>
        private readonly List<string> mCommonChangesRisingProprties = new List<string>()
        {
            nameof(TaskStringContent),
            nameof(PointScore),
        };

        #endregion

        #region Abstract Properties

        /// <summary>
        /// All properties that can cause any unsaved changes
        /// </summary>
        protected abstract List<string> SpecificChangesRisingProperties { get; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The error message to display
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Indicates if error message should be visible
        /// </summary>
        public bool IsErrorMessageVisible => !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Indicates if there are some unsaved changes
        /// </summary>
        public bool AnyUnsavedChanges { get; protected set; }
    
        /// <summary>
        /// Indicates if this viewmodel is in design mode rather than in creation mode
        /// </summary>
        public bool IsInEditMode { get; private set; }

        /// <summary>
        /// The question that is being edited right now, if not in edit mode null
        /// </summary>
        public Question OriginalQuestion { get; private set; }

        #region Editable Fields

        /// <summary>
        /// The string content of the task
        /// </summary>
        public string TaskStringContent { get; set; }

        /// <summary>
        /// Point score for this question
        /// </summary>
        public string PointScore { get; set; }

        #endregion

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// Submits current question
        /// </summary>
        /// <returns>Null if validation falied; otherwise, the question that has been created/edited using this editor</returns>
        public abstract Question Submit();

        /// <summary>
        /// Attaches a question to the viewmodel
        /// </summary>
        /// <param name="Question">The question to attach</param>
        protected virtual void AttachQuestion(Question Question)
        {
            // Save the question
            OriginalQuestion = Question;

            if (OriginalQuestion != null)
            {
                TaskStringContent = OriginalQuestion.Task == null ? "" : OriginalQuestion.Task.StringContent;
                PointScore = OriginalQuestion.Scoring.FullPointScore.ToString();
                IsInEditMode = true;
            }
            else
            {
                IsInEditMode = false;
                AnyUnsavedChanges = true;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseQuestionEditorViewModel(Question QuestionModel)
        {
            AttachQuestion(QuestionModel);

            // Trigger every changes rising properties
            if(IsInEditMode)
            {
                PropertyChanged += CheckForChangesInProperties;

                ImagesEditorViewModel.Instance.ListModified += SetUnsavedChanges;
            }
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Checks for changes in this viewmodel properties
        /// If changes are detected AnyUnsavedChanges flag is set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckForChangesInProperties(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!AnyUnsavedChanges && (mCommonChangesRisingProprties.Contains(e.PropertyName) ||
                                       SpecificChangesRisingProperties.Contains(e.PropertyName)))
                SetUnsavedChanges();
        }

        /// <summary>
        /// Sets unsaved changes to true
        /// </summary>
        // Can't do a anonymous method because we need to unsubscribe from this event later
        private void SetUnsavedChanges()
        {
            AnyUnsavedChanges = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the editor viewmodel corresponding question type
        /// </summary>
        /// <param name="QuestionToLoad">The question to load</param>
        /// <param name="Type">The type of a question to load</param>
        /// <returns>Null: if null question is passed in and no type is specified
        ///          Corresponding viewmodel: if only question passed in Type parameter is ignored
        ///                                   if null question with a concrete type passed in</returns>
        public static BaseQuestionEditorViewModel ToViewModel(Question QuestionToLoad, QuestionType Type = QuestionType.None)
        {
            if (QuestionToLoad == null && Type == QuestionType.None)
                return null;
            else if (QuestionToLoad != null)
                Type = QuestionToLoad.Type;

            switch (Type)
            {
                case QuestionType.MultipleChoice:
                    return new MultipleChoiceQuestionEditorViewModel(QuestionToLoad);

                case QuestionType.MultipleCheckboxes:
                    return new MultipleChoiceQuestionEditorViewModel(QuestionToLoad);

                case QuestionType.SingleTextBox:
                    return new MultipleChoiceQuestionEditorViewModel(QuestionToLoad);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Disposes this viewmodel
        /// </summary>
        public override void Dispose()
        {
            // These events are only available in edit mode
            if (IsInEditMode)
            {
                PropertyChanged -= CheckForChangesInProperties;
                ImagesEditorViewModel.Instance.ListModified -= SetUnsavedChanges;
            }
        }

        #endregion

    }
}
