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

        #region Protected Members

        /// <summary>
        /// The question that is being edited/created right now
        /// </summary>
        protected Question EditedQuestion { get; private set; }

        #endregion

        #region Protected Methods
        
        /// <summary>
        /// Attaches a question to the viewmodel
        /// </summary>
        /// <param name="Question">The question to attach</param>
        protected virtual void AttachQuestion(Question Question)
        {
            // Save the question
            EditedQuestion = Question;

            if (EditedQuestion != null)
            {
                TaskStringContent = EditedQuestion.Task == null ? "" : EditedQuestion.Task.StringContent;
                PointScore = EditedQuestion.Scoring.FullPointScore.ToString();
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Submits current question
        /// </summary>
        /// <returns>Null if validation falied; otherwise, the question that has been created/edited using this editor</returns>
        public abstract Question Submit();
        
        #endregion

        #region Public Commands

        #endregion

        #region Command Methods



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
                    return null;

                case QuestionType.SingleTextBox:
                    return null;

                default:
                    return null;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseQuestionEditorViewModel(Question model = null)
        {
            
        }

        #endregion

    }
}
