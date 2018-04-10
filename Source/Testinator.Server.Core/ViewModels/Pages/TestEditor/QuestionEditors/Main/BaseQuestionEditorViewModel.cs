using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Base view model for question editors
    /// </summary>
    public abstract class BaseQuestionEditorViewModel : BaseViewModel
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

        #endregion

        #endregion

        #region Protected Members

        /// <summary>
        /// The question that is being edited/created right now
        /// </summary>
        protected Question EditedQuestion { get; private set; }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Submits current question
        /// </summary>
        /// <returns>Null if validation falied; otherwise, the question that has been created/edited using this editor</returns>
        public abstract Question Submit();

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to add image to the current task
        /// </summary>
        //public ICommand AddImageCommand { get; private set; }

        #endregion

        #region Command Methods



        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="model">The model of a question to edit. If null creation mode is prsumed</param>
        public BaseQuestionEditorViewModel(Question model = null)
        {
            // Save the question
            EditedQuestion = model;

            if (EditedQuestion != null)
            {
                TaskStringContent = EditedQuestion.Task == null ? "" : EditedQuestion.Task.StringContent;
            }
            
        }

        #endregion

    }
}
