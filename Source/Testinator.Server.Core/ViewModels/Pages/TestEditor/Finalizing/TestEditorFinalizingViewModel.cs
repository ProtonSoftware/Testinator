using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The viewmodel for test editor finalizing page
    /// </summary>
    public class TestEditorFinalizingViewModel : BaseViewModel
    {
        #region Private Properties

        /// <summary>
        /// Indicates if default filename should be used
        /// </summary>
        private bool mDefaultFileName = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the test
        /// </summary>
        public string TestName => IoCServer.TestEditor.CurrentTestName;

        /// <summary>
        /// Duration of the test, already formatted
        /// </summary>
        public string Duration => IoCServer.TestEditor.CurrentDuration;

        /// <summary>
        /// Current tags associated with the test
        /// </summary>
        public string Tags => IoCServer.TestEditor.CurrentTags;

        /// <summary>
        /// Current point score for this test
        /// </summary>
        public string TotalPointsScore => IoCServer.TestEditor.CurrentPointScore.ToString();

        /// <summary>
        /// The number of multiple chocie questions
        /// </summary>
        public int MultipleChoiceQuestionsCount => IoCServer.TestEditor.CurrentQuestionsNumber[QuestionType.MultipleChoice];

        /// <summary>
        /// The number of multiple checkboxes questions
        /// </summary>
        public int MultipleCheckBoxesQuestionsCount => IoCServer.TestEditor.CurrentQuestionsNumber[QuestionType.MultipleCheckboxes];

        /// <summary>
        /// The number of single testbox questions
        /// </summary>
        public int SingleTextBoxQuestionsCount => IoCServer.TestEditor.CurrentQuestionsNumber[QuestionType.SingleTextBox];

        /// <summary>
        /// File name for this test
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Indicates if default name for the test will be used
        /// </summary>
        public bool DefaultFileName
        {
            get => mDefaultFileName;
            set
            {
                // In edit mode file name cannot be changed
                // TODO: save as... but after file classes rework
                if (IoCServer.TestEditor.IsInEditMode)
                    return;

                mDefaultFileName = value;
                if (value)
                    FileName = GetFreeFileName();

                OnPropertyChanged(nameof(DefaultFileName));
            }
        }

        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Indicates if the error message should be visible
        /// </summary>
        public bool IsErrorMessageVisible => !string.IsNullOrEmpty(ErrorMessage);

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to save the test and finish work
        /// </summary>
        public ICommand FinishCommand { get; private set; }

        /// <summary>
        /// The command to go back to the previous page
        /// </summary>
        public ICommand GoPreviousPageCommand { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Finishes the work and saves the test
        /// </summary>
        private void Finish()
        {
            if (!DefaultFileName)
            {
                if (!ValidateFileName(FileName + ".dat"))
                {
                    ErrorMessage = "Nieprawidłowa nazwa pliku";
                    return;
                }
            }

            if(IoCServer.TestEditor.Save(FileName))
                IoCServer.TestEditor.FinishAndClose();
        }

        /// <summary>
        /// Goes to the previous page
        /// </summary>
        private void GoPreviousPage()
        {
            IoCServer.TestEditor.GoPreviousPage();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorFinalizingViewModel()
        {
            GoPreviousPageCommand = new RelayCommand(GoPreviousPage);
            FinishCommand = new RelayCommand(Finish);

            if (IoCServer.TestEditor.IsInEditMode)
                FileName = IoCServer.TestEditor.GetCurrentTestFileName();
            else
                FileName = GetFreeFileName();   
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets free file name for the test
        /// </summary>
        /// <returns></returns>
        private string GetFreeFileName()
        {
            // TODO: improve it after file class rework
            return "Test" + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Validates file name and check if it can be used to save the test
        /// </summary>
        /// <param name="fileName">File name to check</param>
        /// <returns>True if file is valid and free; otherwise, false;</returns>
        private bool ValidateFileName(string fileName)
        {
            try
            {
                var fi = new FileInfo(FileName);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
