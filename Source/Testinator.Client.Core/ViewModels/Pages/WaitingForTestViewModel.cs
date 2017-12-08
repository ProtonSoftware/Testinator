using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for "waiting for test" page
    /// </summary>
    public class WaitingForTestViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current users name
        /// </summary>
        public TextEntryViewModel Name { get; set; }

        /// <summary>
        /// The current users surname
        /// </summary>
        public TextEntryViewModel Surname { get; set; }

        /// <summary>
        /// A flag indicating if we have any test to show
        /// </summary>
        public bool ReceivedTest { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaitingForTestViewModel()
        {
            // Set input data
            Name = new TextEntryViewModel { Label = "Imię", OriginalText = IoCClient.Client.ClientName };
            Surname = new TextEntryViewModel { Label = "Nazwisko", OriginalText = IoCClient.Client.ClientSurname };

            // Listen out for test package
            IoCClient.Application.TestReceived += Application_TestReceived;

            // Fake test for now, TODO: delete it and wait for server to send test
            FakeTest();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when the test from server has arrived
        /// </summary>
        /// <param name="obj"></param>
        private void Application_TestReceived(bool obj)
        {
            // TODO: Randomize question order etc. 
            
            // Create a dummy question 
            var viewmodel1 = new QuestionMultipleCheckboxesViewModel();
            viewmodel1.AttachQuestion(new MultipleCheckboxesQuestion()
            {
                PointScore = 1,
                Task = "Co robi kot?",
                OptionsAndAnswers = new Dictionary<string, bool>() { { "nic", true }, { "miau", true }, { "hau", false } },
            });

            var viewmodel2 = new QuestionMultipleChoiceViewModel();
            viewmodel2.AttachQuestion(new MultipleChoiceQuestion());

            var viewmodel3 = new QuestionSingleTextBoxViewModel();
            viewmodel3.AttachQuestion(new SingleTextBoxQuestion());

            // Change page to the first question page
            //IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleCheckboxes, viewmodel1);
            IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleChoice, viewmodel2);
            //IoCClient.Application.GoToPage(ApplicationPage.QuestionSingleTextBox, viewmodel3);
        }

        /// <summary>
        /// TODO: delete it and wait for server to send test
        /// </summary>
        /// <returns></returns>
        private async void FakeTest()
        {
            // Do a task
            await TaskFakeTest();
        }

        private async Task TaskFakeTest()
        {
            // Wait a small delay
            await Task.Delay(1500);

            // Fake test
            IoCClient.Application.Test = new Test();
        }

        #endregion
    }
}
