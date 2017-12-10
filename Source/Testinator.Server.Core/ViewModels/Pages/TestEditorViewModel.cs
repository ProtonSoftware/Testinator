using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for test editor(creator) page
    /// </summary>
    public class TestEditorViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The name of the test user wants to create
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The time this test is going to take to complete (as string)
        /// </summary>
        public string Duration { get; set; } = "";

        /// <summary>
        /// The test itself, builded step by step
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// A flag indicating if invalid data error should be shown
        /// </summary>
        public bool InvalidDataError { get; set; } = false;

        /// <summary>
        /// The type of the question being added right now
        /// </summary>
        public QuestionType QuestionBeingAddedType { get; set; } = QuestionType.None;

        #endregion

        #region Commands

        /// <summary>
        /// The command to change page to question adding page
        /// </summary>
        public ICommand AddingQuestionsPageChangeCommand { get; private set; }

        /// <summary>
        /// The command to submit newly created question and add it to the test
        /// </summary>
        public ICommand SubmitQuestionCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorViewModel()
        {
            // Create commands
            AddingQuestionsPageChangeCommand = new RelayCommand(ChangePage);
            SubmitQuestionCommand = new RelayCommand(SubmitQuestion);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to question adding page
        /// </summary>
        private void ChangePage()
        {
            // Disable previous errors
            InvalidDataError = false;

            // Check if input data is valid
            if (this.Name.Length < 3 || !Int32.TryParse(Duration, out int time) || time > 500 || time <= 0)
            {
                // Show error and dont change page
                InvalidDataError = true;
                return;
            }

            // Save this view model
            var viewModel = new TestEditorViewModel();
            viewModel.Name = this.Name;
            viewModel.Duration = this.Duration;
            viewModel.Test = this.Test;

            // Pass it to the next page
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorAddQuestions, viewModel);
        }

        /// <summary>
        /// Submits newly created question and adds it to the test
        /// </summary>
        private void SubmitQuestion()
        {
            return;
        }

        #endregion

        // Test/Dummy/Placeholder data
        /*
        public string Name { get; set; } = "nazwa";
        public string Duration { get; set; } = "32";
        public ICommand SubmitCommand { get; set; }

        public string Pytanie1 { get; set; } = "dddd?";
        public string A1 { get; set; } = "sa";
        public string B1 { get; set; } = "sa";
        public string C1 { get; set; } = "ds";
        public string D1 { get; set; } = "3";
        public string PoprawnaIdx1 { get; set; } = "1";
        public string Punkty1 { get; set; } = "1";

        public string Pytanie2 { get; set; } = "sdsdsd";
        public string Odp2 { get; set; } = "sss";
        public string Punkty2 { get; set; } = "1";

        public string Pytanie3 { get; set; } = "adsfdasf";
        public string A3 { get; set; } = "fdsf";
        public string B3 { get; set; } = "dfdsf";
        public string C3 { get; set; } = "sdfsd";
        public string D3 { get; set; } = "fsdfs";
        public string PoprawnaIdx3 { get; set; } = "1";
        public string Punkty3 { get; set; } = "12";

        public string Pytanie4 { get; set; } = "agsdfdg";
        public bool A4 { get; set; } = true;
        public bool B4 { get; set; } = false;
        public bool C4 { get; set; } = true;
        public string OptionA4 { get; set; } = "opcja1";
        public string OptionB4 { get; set; } = "opcja2";
        public string OptionC4 { get; set; } = "opcja3";
        public string Punkty4 { get; set; } = "12";

        public string Pytanie5 { get; set; } = "dfklsdf";
        public string A5 { get; set; } = "asas";
        public string B5 { get; set; } = "sas";
        public string C5 { get; set; } = "ggg";
        public string D5 { get; set; } = "ffdf";
        public string PoprawnaIdx5 { get; set; } = "2";
        public string Punkty5 { get; set; } = "11";

        private void Submit()
        {
            var time = new TimeSpan();
            int pkt1;
            int pkt2;
            int pkt3;
            int pkt4;
            int pkt5;
                    
            int pop1;
            int pop3;
            int pop5;
            
            try
            {
                time = new TimeSpan(0, Int32.Parse(Duration), 0);
                pkt1 = Int32.Parse(Punkty1);
                pkt2 = Int32.Parse(Punkty1);
                pkt3 = Int32.Parse(Punkty1);
                pkt4 = Int32.Parse(Punkty1);
                pkt5 = Int32.Parse(Punkty1);

                pop1 = Int32.Parse(PoprawnaIdx1);
                pop3 = Int32.Parse(PoprawnaIdx3);
                pop5 = Int32.Parse(PoprawnaIdx5);

            }
            catch
            {
                return;
            }

            var q1 = new MultipleChoiceQuestion();
            var q2 = new SingleTextBoxQuestion();
            var q3 = new MultipleChoiceQuestion();
            var q4 = new MultipleCheckboxesQuestion();
            var q5 = new MultipleChoiceQuestion();

            try
            {
                q1 = new MultipleChoiceQuestion()
                {
                    PointScore = pkt1,
                    Task = Pytanie1,
                    CorrectAnswerIndex = pop1,
                    Options = new List<string>() { A1, B1, C1, D1},
                };
            }
            catch (QuestionException ex)
            {
            }

            try
            {
                q2 = new SingleTextBoxQuestion()
                {
                    PointScore = pkt2,
                    Task = Pytanie2,
                    CorrectAnswer = "pomidor",
                };
            }
            catch (QuestionException ex)
            {
            }

            try
            {
                q3 = new MultipleChoiceQuestion()
                {
                    PointScore = pkt3,
                    Task = Pytanie3,
                    CorrectAnswerIndex = pop3,
                    Options = new List<string>() { A3, B3, C3, D3 },
                };
            }
            catch (QuestionException ex)
            {
            }

            try
            {
                q4 = new MultipleCheckboxesQuestion()
                {
                    PointScore = pkt4,
                    Task = Pytanie4,
                    OptionsAndAnswers = new Dictionary<string, bool>()
                    {
                        { OptionA4, A4 },
                        { OptionB4, B4 },
                        { OptionC4, C4 },
                    },
                
                };
            }
            catch (QuestionException ex)
            {
            }

            try
            {
                q5 = new MultipleChoiceQuestion()
                {
                    PointScore = pkt5,
                    Task = Pytanie5,
                    CorrectAnswerIndex = pop5,
                    Options = new List<string>() { A5, B5, C5, D5 },
                };
            }
            catch (QuestionException ex)
            {
            }

            var test = new Test()
            {
                Name = Name,
                Duration = time,
                Questions = new List<Question>() { q1, q2, q3, q4 }
            };
            test.AddQuestion(q5);
        }

    
        */
    }
}
