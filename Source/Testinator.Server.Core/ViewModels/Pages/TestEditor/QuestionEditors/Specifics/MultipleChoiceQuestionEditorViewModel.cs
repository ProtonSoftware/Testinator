using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Viewmodel for specific question editor page, multiplechoice question in this case
    /// </summary>
    public class MultipleChoiceQuestionEditorViewModel : BaseQuestionEditorViewModel
    {
        #region Protected Memebers

        /// <summary>
        /// Properties that can cause any unsaved changes
        /// </summary>
        protected override List<string> SpecificChangesRisingProperties => new List<string>()
        {
            nameof(AnswerA),
            nameof(AnswerB),
            nameof(AnswerC),
            nameof(AnswerD),
            nameof(AnswerE),
            nameof(ShouldAnswerCBeVisible),
            nameof(ShouldAnswerDBeVisible),
            nameof(ShouldAnswerEBeVisible),
            nameof(CorrectAnswerIndex),
        };

        #endregion

        #region Public Properties

        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string AnswerE { get; set; }

        public bool ShouldAnswerCBeVisible { get; set; } = false;
        public bool ShouldAnswerDBeVisible { get; set; } = false;
        public bool ShouldAnswerEBeVisible { get; set; } = false;

        /// <summary>
        /// Index of the correct answer
        /// </summary>
        public int CorrectAnswerIndex { get; set; }
        
        /// <summary>
        /// Indicates if the user can add options to the question
        /// </summary>
        public bool CanAddOptions { get; private set; } = true;

        /// <summary>
        /// Indicates if the user can remove options to the question
        /// </summary>
        public bool CanRemoveOptions { get; private set; } 

        #endregion

        #region Private Members

        /// <summary>
        /// The builder for this question
        /// </summary>
        private MultipleChoiceQuestionBuilder Builder { get; set; }

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

        /// <summary>
        /// The command to select correct answer
        /// </summary>
        public ICommand SelectCorrectAnswerCommand { get; private set; }

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
                if (CorrectAnswerIndex == 4)
                    CorrectAnswerIndex = 3;
            }
            else if (ShouldAnswerDBeVisible)
            {
                ShouldAnswerDBeVisible = false;
                if (CorrectAnswerIndex == 3)
                    CorrectAnswerIndex = 2;
            }
            else if (ShouldAnswerCBeVisible)
            {
                ShouldAnswerCBeVisible = false;
                CanRemoveOptions = false;
                if (CorrectAnswerIndex == 2)
                    CorrectAnswerIndex = 1;

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

        /// <summary>
        /// Selects correct answer for the question
        /// </summary>
        /// <param name="obj">Answer index</param>
        private void SelectCorrectAnswer(object obj)
        {
            var index = int.Parse((string)obj);

            CorrectAnswerIndex = index;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Attaches a question to the viewmodel
        /// </summary>
        /// <param name="Question">The question to attach. If null no properties should be loaded</param>
        protected override void AttachQuestion(Question Question)
        {
            base.AttachQuestion(Question);

            if (Question == null)
                return;

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
            Builder = new MultipleChoiceQuestionBuilder(question);

            // Cannot think of a better way to do that
            for (var i = 0; i < question.Options.Count; i++)
            {
                switch (i)
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
                        CanRemoveOptions = true;
                        break;

                    case 3:
                        AnswerD = question.Options[i];
                        ShouldAnswerDBeVisible = true;
                        break;

                    case 4:
                        AnswerE = question.Options[i];
                        ShouldAnswerEBeVisible = true;
                        CanAddOptions = false;
                        break;
                }

            }

            CorrectAnswerIndex = question.CorrectAnswerIndex;
        }

        /// <summary>
        /// Submits this form
        /// </summary>
        /// <returns>Null if the question cannot be returned</returns>
        public override Question Submit()
        {
            try
            {
                var task = new TaskContent(TaskStringContent);
                Builder.AddTask(task);

                var questionsList = new List<string>()
                {
                    AnswerA,
                    AnswerB,
                };

                if (string.IsNullOrEmpty(AnswerA))
                    throw new Exception("Odpowiedź A zawiera puste pole");

                if (string.IsNullOrEmpty(AnswerB))
                    throw new Exception("Odpowiedź B zawiera puste pole");

                if (ShouldAnswerCBeVisible)
                {
                    if (string.IsNullOrEmpty(AnswerC))
                        throw new Exception("Odpowiedź C zawiera puste pole");
                    else
                        questionsList.Add(AnswerC);
                }

                if (ShouldAnswerDBeVisible)
                {
                    if (string.IsNullOrEmpty(AnswerD))
                        throw new Exception("Odpowiedź D zawiera puste pole");
                    else
                        questionsList.Add(AnswerD);
                }

                if (ShouldAnswerEBeVisible)
                {
                    if (string.IsNullOrEmpty(AnswerE))
                        throw new Exception("Odpowiedź E zawiera puste pole");
                    else
                        questionsList.Add(AnswerE);
                }

                Builder.AddOptions(questionsList);

                Builder.AddCorrectAnswer(CorrectAnswerIndex);

                if (!int.TryParse(string.IsNullOrEmpty(PointScore) ? null : PointScore, out var pointsInt))
                    throw new Exception("Nieprawidłowa wartość w polu liczba punktów");

                Builder.AddScoring(new Scoring(ScoringMode.FullAnswer, pointsInt));

                return Builder.GetResult();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }   
            
            return null;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Model">The model for this view. If null is passed in creating mode is enabled</param>
        public MultipleChoiceQuestionEditorViewModel(Question Model) : base(Model)
        {
            RemoveAnswerCommand = new RelayCommand(RemoveAnswer);
            AddAnswerCommand = new RelayCommand(AddAnswer);
            SelectCorrectAnswerCommand = new RelayParameterizedCommand(SelectCorrectAnswer);
            Builder = new MultipleChoiceQuestionBuilder();
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
