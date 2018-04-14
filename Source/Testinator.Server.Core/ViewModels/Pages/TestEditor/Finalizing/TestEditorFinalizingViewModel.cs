using System;
using System.Collections.Generic;
using System.Linq;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The viewmodel for test editor finalizing page
    /// </summary>
    public class TestEditorFinalizingViewModel : BaseViewModel
    {
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
        public string FileName { get; private set; }

        /// <summary>
        /// Indicates if default name for the test will be used
        /// </summary>
        public bool DefaultFileName { get; private set; }

        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Indicates if the error message should be visible
        /// </summary>
        public bool IsErrorMessageVisible => !string.IsNullOrEmpty(ErrorMessage);

        #endregion


        public TestEditorFinalizingViewModel()
        {
            
        }
    }
}
