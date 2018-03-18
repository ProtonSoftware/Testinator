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

        public List<QuestionsViewItemViewModel> QuestionsViewData { get; private set; } = new List<QuestionsViewItemViewModel>();

        /// <summary>
        /// The name of the test the users took
        /// </summary>
        public string TestName => mTestResults.Test.Name;

        /// <summary>
        /// The current page of the subpage
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.TestResultsStudentsView;

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

        #endregion

        #region Constructor

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
        }

        #endregion
    }
}
