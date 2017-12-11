using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        /// The name of the test
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// The duration of the test
        /// </summary>
        public TimeSpan TestDuration { get; set; }

        /// <summary>
        /// How much score can user get from this test
        /// </summary>
        public string TestPossibleScore { get; set; }

        /// <summary>
        /// How many question this test has
        /// </summary>
        public int TestQuestionCount { get; set; }

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
        private void Application_TestReceived(Test test)
        {
            // Set the properties of the test we've received\
            ReceivedTest = true;
            TestName = test.Name;
            TestDuration = test.Duration;
            // TestPossibleScore = test.;
            // TestQuestionCount = test.;

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
            //IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleChoice, viewmodel2);
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

            // Dummy test
            var test = new Test()
            {
                Duration = new TimeSpan(0, 30, 0),
                Name = "Sample name",
            };

            var q1 = new MultipleChoiceQuestion()
            {
                Task = "Co robi kot?",
                PointScore = 1,
                Options = new List<string>()
                {
                    "miau",
                    "hau",
                    "xaxaxaxaxaxax",
                },
                CorrectAnswerIndex = 1,
            };

            var q2 = new MultipleChoiceQuestion()
            {
                Task = "Co robi kot2222?",
                PointScore = 1,
                Options = new List<string>()
                {
                    "miau2222",
                    "hau2222",
                    "xaxaxaxaxaxax2222",
                },
                CorrectAnswerIndex = 1,
            };

            var q3 = new MultipleChoiceQuestion()
            {
                Task = "Co robi kot333?",
                PointScore = 1,
                Options = new List<string>()
                {
                    "miau333",
                    "hau3333",
                    "xaxaxaxaxaxax333",
                },
                CorrectAnswerIndex = 1,
            };

            var q4 = new MultipleChoiceQuestion()
            {
                Task = "Co robi kot4444?",
                PointScore = 1,
                Options = new List<string>()
                {
                    "miau444",
                    "hau4444",
                    "xaxaxaxaxaxax4444",
                },
                CorrectAnswerIndex = 1,
            };

            var a1 = new MultipleChoiceAnswer(1);

            test.AddQuestion(q1);
            test.AddQuestion(q2);
            test.AddQuestion(q3);
            test.AddQuestion(q4);


            if (DataPackageDescriptor.TryConvertToBin(out byte[] data, new DataPackage(PackageType.TestForm, test)))
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("sample.dat", FileMode.Create)))
                {
                    writer.Write(data);
                }
            }

            IoCClient.TestHost.BindTest(test);
            IoCClient.TestHost.GoNextQuestion();

        }

        #endregion
    }
}
