using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model of the resilts details page
    /// </summary>
    public class TestResultsDetailsViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The results this viewmodel is based on
        /// </summary>
        private TestResults mTestResults = new TestResults();

        #endregion

        #region Public Properties

        /// <summary>
        /// Used to display data in students view mode
        /// </summary>
        public List<ClientModelSerializable> StudentsViewData { get; private set; } = new List<ClientModelSerializable>();

        /// <summary>
        /// Used to display data in questions view mode
        /// </summary>
        public List<QuestionsViewItemViewModel> QuestionsViewData { get; private set; } = new List<QuestionsViewItemViewModel>();

        /// <summary>
        /// The name of the test the users took
        /// </summary>
        public string TestName => mTestResults.Test.Name;
        
        /// <summary>
        /// The current subpage
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.TestResultsStudentsView;

        #endregion

        #region Commands

        /// <summary>
        /// The command to go back to the previous page
        /// </summary>
        public ICommand ReturnCommand { get; private set; }

        /// <summary>
        /// Changes the view to the students view
        /// </summary>
        public ICommand ChangeViewStudentsCommand { get; private set; }

        /// <summary>
        /// Changes the view to the questions view
        /// </summary>
        public ICommand ChangeViewQuestionsCommand { get; private set; }

        /// <summary>
        /// Changes to view to students answer view 
        /// </summary>
        public ICommand ShowAnswersCommand { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsDetailsViewModel()
        {
            // Create commands
            CreateCommands();
        }

        /// <summary>
        /// Constructs the viewmodel and loads it's properties with the given data
        /// </summary>
        /// <param name="results">Data to be loaded</param>
        public TestResultsDetailsViewModel(TestResults results)
        {
            // Create commands
            CreateCommands();

            LoadData(results);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Goes back to the previous page
        /// </summary>
        private void ReturnPreviousPage()
        {
            IoCServer.Application.GoToPage(ApplicationPage.TestResultsInitial);
        }

        /// <summary>
        /// Changes the view to the students view
        /// </summary>
        private void ChangeViewStudents()
        {
            CurrentPage = ApplicationPage.TestResultsStudentsView;
            OnPropertyChanged(nameof(CurrentPage));
        }

        /// <summary>
        /// Changes the view to the questions view
        /// </summary>
        private void ChangeViewQuestions()
        {
            CurrentPage = ApplicationPage.TestResultsQuestionsView;
            OnPropertyChanged(nameof(CurrentPage));
        }

        /// <summary>
        /// Changes the view to student's answers view
        /// </summary>
        /// <param name="client"></param>
        private void ChangeViewAnswers(object client)
        {
            var Client = client as ClientModelSerializable;

            if (Client == null)
                return;

            var UserAnswers = mTestResults.Results[Client];

            // Total point score
            var totalScore = 0;

            // Log what we are doing
            IoCServer.Logger.Log("Calculating user's score");

            // Get the questions from the test
            var Questions = mTestResults.Test.Questions;

            var QuestionViewModels = new List<BaseViewModel>();

            // We can iterate like this because the question list and answer list are in the same order
            for (var i = 0; i < Questions.Count; i++)
            {
                // Based on question type...
                switch (Questions[i].Type)
                {
                    case QuestionType.MultipleChoice:
                        {
                            // Create local variables for the answer and correct answer boolean for the further use
                            MultipleChoiceAnswer multipleChoiceAnswer = null;
                            var isAnswerCorrect = false;
                            var multipleChoiceQuestion = Questions[i] as MultipleChoiceQuestion;

                            // Try to get answer in the try/catch in case the answer doesn't exist
                            try
                            {
                                // Cast the answer and question objects
                                // NOTE: if the answer doesn't exist exception is thrown here
                                multipleChoiceAnswer = UserAnswers[i] as MultipleChoiceAnswer;

                                isAnswerCorrect = multipleChoiceQuestion.IsAnswerCorrect(multipleChoiceAnswer);

                                // Check if user has answered correctly
                                if (isAnswerCorrect)
                                    // Give them points for this question
                                    totalScore += multipleChoiceQuestion.PointScore;
                            }
                            // Catch the exception but let the answer to be saved
                            catch (ArgumentOutOfRangeException) { }

                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionMultipleChoiceViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = multipleChoiceAnswer,
                                IsReadOnly = true,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion(multipleChoiceQuestion);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;

                    case QuestionType.MultipleCheckboxes:
                        {
                            // Create local variables for the answer and correct answer boolean for the further use
                            MultipleCheckboxesAnswer multipleCheckboxesAnswer = null;
                            var isAnswerCorrect = false;
                            var multipleCheckboxesQuestion = Questions[i] as MultipleCheckboxesQuestion;

                            // Try to get answer in the try/catch in case the answer doesn't exist
                            try
                            {
                                // Cast the answer and question objects
                                // NOTE: if the answer doesn't exist exception is thrown here
                                multipleCheckboxesAnswer = UserAnswers[i] as MultipleCheckboxesAnswer;

                                isAnswerCorrect = multipleCheckboxesQuestion.IsAnswerCorrect(multipleCheckboxesAnswer);

                                // Check if user has answered correctly
                                if (isAnswerCorrect)
                                    // Give them points for this question
                                    totalScore += multipleCheckboxesQuestion.PointScore;
                            }
                            // Catch the exception but let the answer to be saved
                            catch (ArgumentOutOfRangeException) { }

                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionMultipleCheckboxesViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = multipleCheckboxesAnswer,
                                IsReadOnly = true,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion(multipleCheckboxesQuestion);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;

                    case QuestionType.SingleTextBox:
                        {
                            // Create local variables for the answer and correct answer boolean for the further use
                            SingleTextBoxAnswer singleTextBoxAnswer = null;
                            var isAnswerCorrect = false;
                            var singleTextBoxQuestion = Questions[i] as SingleTextBoxQuestion;

                            // Try to get answer in the try/catch in case the answer doesn't exist
                            try
                            {
                                // Cast the answer and question objects
                                // NOTE: if the answer doesn't exist exception is thrown here
                                singleTextBoxAnswer = UserAnswers[i] as SingleTextBoxAnswer;

                                isAnswerCorrect = singleTextBoxQuestion.IsAnswerCorrect(singleTextBoxAnswer);

                                // Check if user has answered correctly
                                if (isAnswerCorrect)
                                    // Give them multipleCheckboxesAnswer for this question
                                    totalScore += singleTextBoxQuestion.PointScore;
                            }
                            // Catch the exception but let the answer to be saved
                            catch (ArgumentOutOfRangeException) { }

                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionSingleTextBoxViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = singleTextBoxAnswer?.Answer,
                                IsReadOnly = true,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion(singleTextBoxQuestion);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;
                }
            }

            var questionsViewmodel = new ResultQuestionsViewModel(QuestionViewModels)
            {
                Name = $"{Client.ClientName} {Client.ClientSurname}",
                Results = mTestResults,
            };

            IoCServer.Application.GoToPage(ApplicationPage.ResultQuestions, questionsViewmodel);

            questionsViewmodel.ShowFirstQuestion();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Loads this viewmodel with given data
        /// </summary>
        /// <param name="value"></param>
        private void LoadData(TestResults value)
        {
            if (value == null)
                return;

            StudentsViewData = value.Clients;
            mTestResults = value;

            CreateQuestionsViewData();
        }

        /// <summary>
        /// Creates data for the quesitons-view mode
        /// </summary>
        private void CreateQuestionsViewData()
        {
            // Clear any junk
            QuestionsViewData.Clear();


            foreach (var student in mTestResults.Results.Keys)
            {
                // Get the answers given by this user
                var answers = mTestResults.Results[student];

                if (answers != null)
                    answers = answers.OrderBy(x => x.ID).ToList();

                // Create a viewmodel for them
                var viewmodel = new QuestionsViewItemViewModel()
                {
                    Name = student.ClientName,
                    Surname = student.ClientSurname,
                };


                // TODO: damnn, this really needs a rework. Dirty, but works. 
                // I dont even bother to comment this because it will be replaced anyway.

                var i = 0;
                foreach(var question in mTestResults.Test.Questions)
                {
                    var scoredPoints = 0;
                    try
                    {
                        switch (question.Type)
                        {
                            case QuestionType.MultipleCheckboxes:
                                if ((question as MultipleCheckboxesQuestion).IsAnswerCorrect(answers[i] as MultipleCheckboxesAnswer))
                                    scoredPoints = (question as MultipleCheckboxesQuestion).PointScore;
                                break;

                            case QuestionType.MultipleChoice:
                                if ((question as MultipleChoiceQuestion).IsAnswerCorrect(answers[i] as MultipleChoiceAnswer))
                                    scoredPoints = (question as MultipleChoiceQuestion).PointScore;
                                break;

                            case QuestionType.SingleTextBox:
                                if ((question as SingleTextBoxQuestion).IsAnswerCorrect(answers[i] as SingleTextBoxAnswer))
                                    scoredPoints = (question as SingleTextBoxQuestion).PointScore;
                                break;
                        }
                    }
                    catch(Exception)
                    {
                        scoredPoints = 0;
                    }
                    finally { }

                    i++;

                    viewmodel.QuestionsPoints.Add(scoredPoints);
                }

                QuestionsViewData.Add(viewmodel);

            }
        }

        /// <summary>
        /// Creates commands in this view model
        /// </summary>
        private void CreateCommands()
        {
            ReturnCommand = new RelayCommand(ReturnPreviousPage);
            ChangeViewStudentsCommand = new RelayCommand(ChangeViewStudents);
            ChangeViewQuestionsCommand = new RelayCommand(ChangeViewQuestions);
            ShowAnswersCommand = new RelayParameterizedCommand(ChangeViewAnswers);
        }

        #endregion
    }
}
