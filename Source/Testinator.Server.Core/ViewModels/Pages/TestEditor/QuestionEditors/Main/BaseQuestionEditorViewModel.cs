using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Base view model for question editors
    /// </summary>
    public abstract class BaseQuestionEditorViewModel: BaseViewModel
    {
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

        #region Abstract/Virual Methods

        /// <summary>
        /// Submits current question
        /// </summary>
        /// <returns>Null if validation falied; otherwise, the question that has been created/edited using this editor</returns>
        public abstract Question Submit();

        /// <summary>
        /// Attaches a question to the viewmodel
        /// </summary>
        /// <param name="Question">The question to attach</param>
        public virtual void AttachQuestion(Question Question)
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
                OriginalQuestion = null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the editor viewmodel corresponding question type
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static BaseQuestionEditorViewModel ToViewModel(QuestionType Type)
        {
            switch (Type)
            {
                case QuestionType.None:
                    return null;

                case QuestionType.MultipleChoice:
                    return new MultipleChoiceQuestionEditorViewModel();

                case QuestionType.MultipleCheckboxes:
                    return new MultipleChoiceQuestionEditorViewModel();
                    return null;

                case QuestionType.SingleTextBox:
                    return new MultipleChoiceQuestionEditorViewModel();
                    return null;

                default:
                    return null;
            }
        }
        
        #endregion

    }
}
