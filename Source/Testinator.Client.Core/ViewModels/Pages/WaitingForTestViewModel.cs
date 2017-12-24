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
        public string TestName => IoCClient.TestHost.CurrentTest.Name;

        /// <summary>
        /// The duration of the test
        /// </summary>
        public TimeSpan TestDuration => IoCClient.TestHost.CurrentTest.Duration;

        /// <summary>
        /// How much score can user get from this test
        /// </summary>
        public int TestPossibleScore => IoCClient.TestHost.CurrentTest.MaxPossibleScore();

        /// <summary>
        /// A flag indicating if we have any test to show,
        /// to show corresponding content in the WaitingPage
        /// </summary>
        public bool IsTestReceived => IoCClient.TestHost.IsTestReceived;

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
            IoCClient.TestHost.OnTestReceived += TestHost_OnTestReceived;
            //FakeTest();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates this view model. As it's properties are bound to the IoC, changing 
        /// something in IoC doesn't fire PropertyChanged event in this view model.
        /// Therefore, properties need to be updated manually
        /// </summary>
        public void Update()
        {
            OnPropertyChanged(nameof(IsTestReceived));
            OnPropertyChanged(nameof(TestName));
            OnPropertyChanged(nameof(TestDuration));
            OnPropertyChanged(nameof(TestPossibleScore));
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when a test is recived so when can update the view
        /// </summary>
        private void TestHost_OnTestReceived()
        {
            // Update the view
            Update();
        }
        
        /// <summary>
        /// TODO: delete it and wait for server to send test
        /// </summary>
        /// <returns></returns>
        private async void FakeTest()
        {
            await Task.Delay(2000);
            // Do a task
            await TaskFakeTest();
        }

        private async Task TaskFakeTest()
        {
            // Wait a small delay
            await Task.Delay(1000);

            // Create a dummy test
            var test = new Test()
            {
                Duration = new TimeSpan(0, 0, 5),
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

            int max = test.MaxPossibleScore();


            /*if (DataPackageDescriptor.TryConvertToBin(out byte[] data, new DataPackage(PackageType.TestForm, test)))
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("sample.dat", FileMode.Create)))
                {
                    writer.Write(data);
                }
            }*/


            test.Grading.UpdateMark(Marks.B, 25, 20);
            test.Grading.UpdateMark(Marks.C, 19, 15);
            test.Grading.UpdateMark(Marks.D, 14, 10);
            test.Grading.UpdateMark(Marks.E, 9, 5);
            test.Grading.UpdateMark(Marks.F, 4, 0);

            var a = test.Grading.ConvertToPercentage();
            var b = a.ToPoints(25);

            var sddds = test.Grading.GetMark(6);

            var grad = new GradingPercentage();

            FileWriters.XmlWriter.SaveGrading("sample", grad);

            IoCClient.TestHost.BindTest(test);

            await Task.Delay(2000);
            IoCClient.TestHost.Start();
            //IoCClient.TestHost.GoNextQuestion();

        }

        #endregion
    }
}
