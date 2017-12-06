using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    public class TestEditorViewModel : BaseViewModel
    {

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
        public string A4 { get; set; } = "fdf";
        public string B4 { get; set; } = "fff";
        public string C4 { get; set; } = "as";
        public string D4 { get; set; } = "oweo";
        public string PoprawnaIdx4 { get; set; } = "3";
        public string Punkty4 { get; set; } = "12";

        public string Pytanie5 { get; set; } = "dfklsdf";
        public string A5 { get; set; } = "asas";
        public string B5 { get; set; } = "sas";
        public string C5 { get; set; } = "ggg";
        public string D5 { get; set; } = "ffdf";
        public string PoprawnaIdx5 { get; set; } = "2";
        public string Punkty5 { get; set; } = "11";

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorViewModel()
        {
            SubmitCommand = new RelayCommand(Submit);
        }

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
            int pop4;
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
                pop4 = Int32.Parse(PoprawnaIdx4);
                pop5 = Int32.Parse(PoprawnaIdx5);

            }
            catch
            {
                return;
            }
            try
            {
                var q1 = new MultipleChoiceQuestion()
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

            /*var q2 = new Question(QuestionTypes.SingleTextBox, pkt2)
            {
                Content = new QuestionContent()
                {
                    Task = Pytanie2,
                    Options = new TextEntryOptions()
                    {
                        CorrectAnswer = Odp2,
                    },
                },
            };

            var q3 = new Question(QuestionTypes.MultipleChoice, pkt3)
            {
                Content = new QuestionContent()
                {
                    Task = Pytanie3,
                    Options = new MultipleChoiceOptions()
                    {
                        CorrectAnswerIndex = pop3,
                        Options = new List<string>() { A3, B3, C3, D3 },
                    },
                },
            };

            var q4 = new Question(QuestionTypes.MultipleChoice, pkt4)
            {
                Content = new QuestionContent()
                {
                    Task = Pytanie4,
                    Options = new MultipleChoiceOptions()
                    {
                        CorrectAnswerIndex = pop1,
                        Options = new List<string>() { A4, B4, C4, D4 },
                    },
                },
            };

            var q5 = new Question(QuestionTypes.MultipleChoice, pkt5)
            {
                Content = new QuestionContent()
                {
                    Task = Pytanie5,
                    Options = new MultipleChoiceOptions()
                    {
                        CorrectAnswerIndex = pop5,
                        Options = new List<string>() { A5, B5, C5, D5 },
                    },
                },
            };

            var test = new Test(Name, time)
            {
                Questions = new List<Question>() { q1, q2, q3, q4, q5 }
            };*/

        }

        #endregion
    }
}
