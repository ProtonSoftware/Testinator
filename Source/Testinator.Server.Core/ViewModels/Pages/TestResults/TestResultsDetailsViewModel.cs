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
        private ServerTestResults mTestResults = new ServerTestResults();

        #endregion

        #region Public Properties

        /// <summary>
        /// Used to display data in students view mode
        /// </summary>
        public List<TestResultsClientModel> StudentsViewData { get; private set; } = new List<TestResultsClientModel>();

        /// <summary>
        /// Used to display data in questions view mode
        /// </summary>
        public List<QuestionsViewItemViewModel> QuestionsViewData { get; private set; } = new List<QuestionsViewItemViewModel>();

        /// <summary>
        /// The name of the test the users took
        /// </summary>
        public string TestName => mTestResults.Test.Info.Name;
        
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

        #region Constructors

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
        public TestResultsDetailsViewModel(ServerTestResults results)
        {
            // Create commands
            CreateCommands();

            // Load given data
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
            if (!(client is TestResultsClientModel Client))
                return;

            var UserAnswers = mTestResults.ClientAnswers[Client] == null ? new List<Answer>() : new List<Answer>(mTestResults.ClientAnswers[Client]);

            var QuestionsOrder = Client.QuestionsOrder;

            var totalScore = 0;

            // Log what we are doing
            IoCServer.Logger.Log("Preparing view data to show user's answers");

            // Get the questions from the test
            var Questions = mTestResults.Test.Questions;

            var QuestionViewModels = new List<BaseViewModel>();

            var QuestionListItemsViewModels = new List<QuestionListItemViewModel>();

            var addedQuestions = Enumerable.Repeat<bool>(false, mTestResults.Test.Questions.Count).ToList();

            // Go through all questions
            for (var i = 0; i < Questions.Count; i++)
            {
                var QuestionIndex = QuestionsOrder[i];

                Answer answer = null;
                var isAnswerCorrect = false;
                var question = Questions[QuestionIndex];

                // Try to get answer in the try/catch in case the answer doesn't exist
                try
                {
                    // Cast the answer and question objects
                    // NOTE: if the answer doesn't exist exception is thrown here
                    answer = UserAnswers[i];

                    var points = question.CheckAnswer(answer);

                    isAnswerCorrect = points != 0;
                    totalScore += points;

                }
                // Catch the exception but let the answer to be saved
                catch (ArgumentOutOfRangeException) { }
                catch (NullReferenceException) { }

                // Based on question type...
                switch (Questions[i].Type)
                {
                    case QuestionType.MultipleChoice:
                        {
                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionMultipleChoiceViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = (MultipleChoiceAnswer)answer,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion((MultipleChoiceQuestion)question);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;

                    case QuestionType.MultipleCheckboxes:
                        {
                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionMultipleCheckboxesViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = (MultipleCheckBoxesAnswer)answer,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion((MultipleCheckBoxesQuestion)question);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;

                    case QuestionType.SingleTextBox:
                        {
                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionSingleTextBoxViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = ((SingleTextBoxAnswer)answer)?.UserAnswer,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion((SingleTextBoxQuestion)question);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;
                }

                QuestionListItemsViewModels.Add(new QuestionListItemViewModel()
                {
                    ID = i,
                    Task = question.Task.StringContent,
                    Type = question.Type,
                    IsSelected = false,
                    IsAnswerCorrect = isAnswerCorrect,
            });
            }

            var questionsViewmodel = new ResultQuestionsViewModel(QuestionViewModels)
            {
                Name = $"{Client.Name} {Client.LastName}",
                Results = mTestResults,
                QuestionListItems = QuestionListItemsViewModels,
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
        private void LoadData(ServerTestResults value)
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

            foreach (var student in mTestResults.ClientAnswers.Keys)
            {
                // Get the answers given by this user
                var answers = mTestResults.ClientAnswers[student];

                // Create a viewmodel for them
                var viewmodel = new QuestionsViewItemViewModel()
                {
                    Name = student.Name,
                    Surname = student.LastName,
                };

                var order = student.QuestionsOrder;

                var i = 0;
                foreach(var question in mTestResults.Test.Questions)
                {
                    var scoredPoints = 0;
                    try
                    {
                        var answer = answers[order.IndexOf(i)];

                        scoredPoints = question.CheckAnswer(answer);
                    }
                    catch
                    {
                        scoredPoints = 0;
                    }

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
