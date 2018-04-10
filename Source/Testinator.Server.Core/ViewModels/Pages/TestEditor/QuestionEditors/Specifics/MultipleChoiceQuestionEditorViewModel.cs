using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Viewmodel for specific question editor page, multiplechoice question in this case
    /// </summary>
    public class MultipleChoiceQuestionEditorViewModel : BaseQuestionEditorViewModel
    {
        #region Public Properties

        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string AnswerE { get; set; }

        public bool ShouldAnswerCBeVisible { get; set; } = false;
        public bool ShouldAnswerDBeVisible { get; set; } = false;
        public bool ShouldAnswerEBeVisible { get; set; } = false;

        public int CorrectAnswerIndex { get; set; } = 0;


        /// <summary>
        /// Indicates if the user can add options to the question
        /// </summary>
        public bool CanAddOptions { get; private set; } = true;

        /// <summary>
        /// Indicates if the user can remove options to the question
        /// </summary>
        public bool CanRemoveOptions { get; private set; } 

        #endregion

        #region Protected Methods

        /// <summary>
        /// Attaches a question to the viewmodel
        /// </summary>
        /// <param name="Question">The question to attach</param>
        protected override void AttachQuestion(Question Question)
        {
            base.AttachQuestion(Question);

            MultipleChoiceQuestion question;
            try
            {
                question = (MultipleChoiceQuestion)Question;
            }
            catch
            {
                throw new NotImplementedException();
            }

            ResetProperties();

            // Cannot think of a better way to do that
            for(var i = 0; i < question.Options.Count; i++)
            {
                switch(i)
                {
                    case 0:
                        AnswerA = question.Options[i];
                        break;

                    case 1:
                        AnswerB = question.Options[i];
                        break;

                    case 2:
                        AnswerC = question.Options[i];
                        ShouldAnswerCBeVisible = true;
                        break;

                    case 3:
                        AnswerD = question.Options[i];
                        ShouldAnswerCBeVisible = true;
                        break;

                    case 4:
                        AnswerE = question.Options[i];
                        ShouldAnswerCBeVisible = true;
                        CanAddOptions = false;
                        break;
                }

            }

            CorrectAnswerIndex = question.CorrectAnswerIndex;
            
        }

        #endregion

        #region Public Commands 

        /// <summary>
        /// Removes the very last answer
        /// </summary>
        public ICommand RemoveAnswerCommand { get; private set; }

        /// <summary>
        /// Adds an additional option. C D or E
        /// </summary>
        public ICommand AddAnswerCommand { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Removes the last answer from the list
        /// </summary>
        private void RemoveAnswer()
        {
            CanAddOptions = true;

            if (ShouldAnswerEBeVisible)
            {
                ShouldAnswerEBeVisible = false;
            }
            else if (ShouldAnswerDBeVisible)
            {
                ShouldAnswerDBeVisible = false;
            }
            else if (ShouldAnswerCBeVisible)
            {
                ShouldAnswerCBeVisible = false;
                CanRemoveOptions = false;
                
            }
                CanAddOptions = true;
        }

        /// <summary>
        /// Adds an answer to the list at the end
        /// </summary>
        private void AddAnswer()
        {
            if (ShouldAnswerCBeVisible)
            {
                if (ShouldAnswerDBeVisible)
                {
                    if (ShouldAnswerEBeVisible)
                        CanAddOptions = false;
                    else
                    {
                        ShouldAnswerEBeVisible = true;
                        CanAddOptions = false;
                    }
                }
                else
                    ShouldAnswerDBeVisible = true;
            }
            else
                ShouldAnswerCBeVisible = true;

            CanRemoveOptions = true;

        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleChoiceQuestionEditorViewModel()
        {
            RemoveAnswerCommand = new RelayCommand(RemoveAnswer);
            AddAnswerCommand = new RelayCommand(AddAnswer);
        }

        public override Question Submit()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Resets this viewmodel's properties
        /// </summary>
        private void ResetProperties()
        {
            AnswerA = "";
            AnswerB = "";
            AnswerC = "";
            AnswerD = "";
            AnswerE = "";
            ShouldAnswerCBeVisible = false;
            ShouldAnswerDBeVisible = false;
            ShouldAnswerEBeVisible = false;
            CorrectAnswerIndex = 0;
            ErrorMessage = "";
            CanAddOptions = true;
            CanRemoveOptions = false;
        }


        #endregion
    }
}
