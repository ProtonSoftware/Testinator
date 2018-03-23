using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for adding new test in TestEditor page
    /// </summary>
    public class TestEditorAddNewTestViewModel : BaseViewModel
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
        /// The text which is displayed on confirm button, either create new or edit existing test
        /// </summary>
        public string ConfirmButtonText => IsEditModeOn ? LocalizationResource.EditTest : LocalizationResource.CreateNewTest;

        /// <summary>
        /// The test itself, builded step by step
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// List of current questions in test converted to an observable collection
        /// </summary>
        public ObservableCollection<Question> Questions { get; set; }

        /// <summary>
        /// The test binary file writer which handles tests saving/deleting from local folder
        /// </summary>
        public BinaryWriter TestFileWriter { get; private set; } = new BinaryWriter(SaveableObjects.Test);

        /// <summary>
        /// Keeps the message of an error to show, if any error occured
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Index of selected question type
        /// None question type set at default, to leave combobox empty at the start
        /// </summary>
        public int QuestionBeingAddedIndex { get; set; } = (int)QuestionType.None;

        /// <summary>
        /// The type of the question being added right now
        /// </summary>
        public QuestionType QuestionBeingAddedType
        {
            get
            {
                // Based on what is chosen in combobox...
                switch(QuestionBeingAddedIndex)
                {
                    case 0: return QuestionType.MultipleChoice;
                    case 1: return QuestionType.MultipleCheckboxes;
                    case 2: return QuestionType.SingleTextBox;

                    default:
                        // None found, return default value
                        return QuestionType.None;
                }
            }
        }

        /// <summary>
        /// The task of the question
        /// </summary>
        public string QuestionTask { get; set; } = "";

        /// <summary>
        /// Indicates if we are editing existing question or creating new one
        /// 0 - means we are creating new one
        /// 1,2,3... - means we are editing question with 1,2,3... index in the list
        /// </summary>
        public int EditingQuestion { get; set; } = 0;
        public bool IsEditModeOn => EditingQuestion == 0 ? false : true;

        #region Multiple Choice Answers

        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string AnswerE { get; set; }

        public int HowManyMultipleChoiceAnswersVisible { get; set; } = 2;
        public string QuestionMultipleChoicePointScore { get; set; }
        public string RightAnswerIdx { get; set; } = "0";

        public bool ShouldAnswerCBeVisible { get; set; } = false;
        public bool ShouldAnswerDBeVisible { get; set; } = false;
        public bool ShouldAnswerEBeVisible { get; set; } = false;

        #endregion

        #region Multiple Checkboxes Answers

        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string Answer5 { get; set; }

        public bool IsAnswer1Right { get; set; }
        public bool IsAnswer2Right { get; set; }
        public bool IsAnswer3Right { get; set; }
        public bool IsAnswer4Right { get; set; }
        public bool IsAnswer5Right { get; set; }

        public int HowManyMultipleCheckboxesAnswersVisible { get; set; } = 2;
        public string QuestionMultipleCheckboxesPointScore { get; set; }

        public bool ShouldAnswer3BeVisible { get; set; } = false;
        public bool ShouldAnswer4BeVisible { get; set; } = false;
        public bool ShouldAnswer5BeVisible { get; set; } = false;

        #endregion

        #region Single TextBox Answers

        public string TextBoxAnswer { get; set; }
        public string QuestionSingleTextBoxPointScore { get; set; }

        #endregion

        #region Criteria Attaching

        /// <summary>
        /// Current grading system attached to the test
        /// </summary>
        public GradingPercentage CurrentGrading { get; set; }

        /// <summary>
        /// Current grading system converted to the points based values
        /// </summary>
        public GradingPoints PointsGrading { get; set; }

        /// <summary>
        /// Indicates if user is editing criteria
        /// </summary>
        public bool IsCriteriaEditModeOn { get; set; } = false;

        public string EditingTopValueA { get; set; }
        public string EditingBottomValueA { get; set; }
        public string EditingTopValueB { get; set; }
        public string EditingBottomValueB { get; set; }
        public string EditingTopValueC { get; set; }
        public string EditingBottomValueC { get; set; }
        public string EditingTopValueD { get; set; }
        public string EditingBottomValueD { get; set; }
        public string EditingTopValueE { get; set; }
        public string EditingBottomValueE { get; set; }
        public string EditingTopValueF { get; set; }
        public string EditingBottomValueF { get; set; }

        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// The command to change page to question adding page
        /// </summary>
        public ICommand AddingQuestionsPageChangeCommand { get; private set; }

        /// <summary>
        /// The command to change page to criteria adding page
        /// </summary>
        public ICommand AddingCriteriaPageChangeCommand { get; private set; }

        /// <summary>
        /// The command to submit newly created question and add it to the test
        /// </summary>
        public ICommand SubmitQuestionCommand { get; private set; }

        /// <summary>
        /// The command to submit newly created test
        /// </summary>
        public ICommand SubmitTestCommand { get; private set; }

        /// <summary>
        /// The command to add new answer to the question
        /// </summary>
        public ICommand AddAnswerCommand { get; private set; }

        /// <summary>
        /// The command to remove an additional answer from the question
        /// </summary>
        public ICommand RemoveAnswerCommand { get; private set; }

        /// <summary>
        /// The command to choose which answer in multiple choice question is the right one
        /// </summary>
        public ICommand ChooseRightAnswerMultipleChoiceCommand { get; private set; }

        /// <summary>
        /// The command to edit question in test
        /// </summary>
        public ICommand EditQuestionCommand { get; private set; }

        /// <summary>
        /// The command to cancel editing question in test
        /// </summary>
        public ICommand CancelEditQuestionCommand { get; private set; }

        /// <summary>
        /// The command to delete question from test
        /// </summary>
        public ICommand DeleteQuestionCommand { get; private set; }

        /// <summary>
        /// The command to select pre-created criteria from the list
        /// </summary>
        public ICommand EditCriteriaCommand { get; private set; }

        /// <summary>
        /// The command to get into editing mode of points criteria
        /// </summary>
        public ICommand EditPointsCriteriaCommand { get; private set; }

        /// <summary>
        /// The command to cancel editing criteria in test
        /// </summary>
        public ICommand CancelEditPointsCommand { get; private set; }

        /// <summary>
        /// The command to submit criteria changes
        /// </summary>
        public ICommand SubmitEditPointsCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor which creates brand new test
        /// </summary>
        public TestEditorAddNewTestViewModel()
        {
            // Create new test
            Test = new Test();

            // Create commands
            CreateCommands();
        }

        /// <summary>
        /// Constructor which allow editing existing test
        /// </summary>
        public TestEditorAddNewTestViewModel(Test test)
        {
            // Catch the test to this view model
            Test = test;

            // Reload questions from this test
            ReloadQuestionsFromTest();

            // Create commands
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to question adding page
        /// </summary>
        private void ChangeQuestionsPage()
        {
            // Remove previous errors
            ErrorMessage = string.Empty;

            try
            {
                // Check if input data is valid
                if (Name.Length < 3 || !int.TryParse(Duration, out var time) || time > 500 || time <= 0)
                {
                    // Show error and dont change page
                    throw new Exception("Niepoprawne parametry testu.");
                }

                // Assign data to the test
                Test.Name = Name;
                Test.Duration = new TimeSpan(0, time, 0);

                // Save the view model and change page
                SaveViewModelAndChangeToNewQuestion();
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Changes page to attaching criteria page
        /// </summary>
        private void ChangeCriteriaPage()
        {
            try
            {
                // Check if test has at least one question
                if (Test.Questions.Count < 1)
                    throw new Exception("Test nie ma pytań.");

                // Check if test's max points allows to create criteria
                if (Test.TotalPointScore < 5)
                    throw new Exception("Punkty za test są niewystarczające.");

                // Save this view model
                var viewModel = new TestEditorAddNewTestViewModel(Test)
                {
                    // Set default points grading 
                    CurrentGrading = new GradingPercentage()
                };

                // Convert default grading to points in this view model
                viewModel.PointsGrading = viewModel.CurrentGrading.ToPoints(Test.TotalPointScore);

                // Set properties to match with criteria in this view model
                viewModel.MatchPropertiesWithCriteria();

                // Pass view model to the next page
                IoCServer.Application.GoToPage(ApplicationPage.TestEditorAttachCriteria, viewModel);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Adds an answer to the question
        /// </summary>
        private void AddAnswer()
        {
            // Check which question type is being created
            if (QuestionBeingAddedType == QuestionType.MultipleChoice)
            {
                // Show next answer
                HowManyMultipleChoiceAnswersVisible++;
                switch (HowManyMultipleChoiceAnswersVisible)
                {
                    case 3:
                        ShouldAnswerCBeVisible = true;
                        break;
                    case 4:
                        ShouldAnswerDBeVisible = true;
                        break;
                    case 5:
                        ShouldAnswerEBeVisible = true;
                        break;
                }
}
            else if (QuestionBeingAddedType == QuestionType.MultipleCheckboxes)
            {
                // Show next answer
                HowManyMultipleCheckboxesAnswersVisible++;
                switch (HowManyMultipleCheckboxesAnswersVisible)
                {
                    case 3:
                        ShouldAnswer3BeVisible = true;
                        break;
                    case 4:
                        ShouldAnswer4BeVisible = true;
                        break;
                    case 5:
                        ShouldAnswer5BeVisible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Removes an answer from the question
        /// </summary>
        private void RemoveAnswer()
        {
            // Check which question type is being created
            if (QuestionBeingAddedType == QuestionType.MultipleChoice)
            {
                // Remove answer based on counter
                switch (HowManyMultipleChoiceAnswersVisible)
                {
                    case 5:
                        ShouldAnswerEBeVisible = false;
                        break;
                    case 4:
                        ShouldAnswerDBeVisible = false;
                        break;
                    case 3:
                        ShouldAnswerCBeVisible = false;
                        break;
                }

                // Decrement the counter
                HowManyMultipleChoiceAnswersVisible--;
            }
            else if (QuestionBeingAddedType == QuestionType.MultipleCheckboxes)
            {
                // Remove answer based on counter
                switch (HowManyMultipleCheckboxesAnswersVisible)
                {
                    case 5:
                        ShouldAnswer5BeVisible = false;
                        break;
                    case 4:
                        ShouldAnswer4BeVisible = false;
                        break;
                    case 3:
                        ShouldAnswer3BeVisible = false;
                        break;
                }

                // Decrement the counter
                HowManyMultipleCheckboxesAnswersVisible--;
            }
        }

        /// <summary>
        /// Submits newly created question and adds it to the test
        /// </summary>
        private void SubmitQuestion()
        {
            try
            { 
                // Based on type
                switch(QuestionBeingAddedType)
                {
                    case QuestionType.MultipleChoice:
                        {       
                            // Create and build a question based on values
                            var question = new MultipleChoiceQuestion();

                            if (QuestionTask.Length < 4)
                                throw new Exception("Treść pytania jest za krótka.");
                            question.Task = QuestionTask;
                            
                            // Try to chuck every answer to the dictionary - it will check if every answer is not empty and unique aswell
                            var errorTestingList = new Dictionary<string,bool>();
                            errorTestingList.Add(AnswerA, false);
                            errorTestingList.Add(AnswerB, false);
                            if (ShouldAnswerCBeVisible) errorTestingList.Add(AnswerC, false);
                            if (ShouldAnswerDBeVisible) errorTestingList.Add(AnswerD, false);
                            if (ShouldAnswerEBeVisible) errorTestingList.Add(AnswerE, false);
                            // Everything is fine - convert it back to the list
                            question.Options = errorTestingList.Keys.ToList();

                            if (!int.TryParse(QuestionMultipleChoicePointScore, out var pointScore))
                                throw new Exception("Zła wartość w polu punkty.");
                            question.PointScore = pointScore;

                            if (!int.TryParse(RightAnswerIdx, out var rightAnswerIdx) || rightAnswerIdx == 0)
                                throw new Exception("Nie wybrano poprawnej odpowiedzi.");
                            question.CorrectAnswerIndex = rightAnswerIdx;

                            // We have our question done, check if we were editing existing question or created new one
                            if (EditingQuestion != 0)
                                // We were editing existing question, replace old one with new one
                                Test.ReplaceQuestion(EditingQuestion, question);
                            else
                                // Its new question, simply add it to the test
                                Test.AddQuestion(question);

                            // Go to adding next question
                            SaveViewModelAndChangeToNewQuestion();
                        }
                        break;

                    case QuestionType.MultipleCheckboxes:
                        {
                            // Create and build a question based on values
                            var question = new MultipleCheckboxesQuestion();

                            if (QuestionTask.Length < 4)
                                throw new Exception("Treść pytania jest za krótka.");
                            question.Task = QuestionTask;
                            
                            question.OptionsAndAnswers = new Dictionary<string, bool>();
                            question.OptionsAndAnswers.Add(Answer1, IsAnswer1Right);
                            question.OptionsAndAnswers.Add(Answer2, IsAnswer2Right);
                            if (ShouldAnswer3BeVisible) question.OptionsAndAnswers.Add(Answer3, IsAnswer3Right);
                            if (ShouldAnswer4BeVisible) question.OptionsAndAnswers.Add(Answer4, IsAnswer4Right);
                            if (ShouldAnswer5BeVisible) question.OptionsAndAnswers.Add(Answer5, IsAnswer5Right);

                            if (!int.TryParse(QuestionMultipleCheckboxesPointScore, out var pointScore))
                                throw new Exception("Zła wartość w polu punkty.");
                            question.PointScore = pointScore;

                            // We have our question done, check if we were editing existing question or created new one
                            if (EditingQuestion != 0)
                                // We were editing existing question, replace old one with new one
                                Test.ReplaceQuestion(EditingQuestion, question);
                            else
                                // Its new question, simply add it to the test
                                Test.AddQuestion(question);

                            // Go to adding next question
                            SaveViewModelAndChangeToNewQuestion();
                        }
                        break;

                    case QuestionType.SingleTextBox:
                        {       
                            // Create and build a question based on values
                            var question = new SingleTextBoxQuestion();

                            if (QuestionTask.Length < 4)
                                throw new Exception("Treść pytania jest za krótka.");
                            question.Task = QuestionTask;

                            if (TextBoxAnswer == string.Empty)
                                throw new Exception("Wpisz poprawną odpowiedź.");
                            question.CorrectAnswer = TextBoxAnswer;

                            if (!int.TryParse(QuestionSingleTextBoxPointScore, out var pointScore))
                                throw new Exception("Zła wartość w polu punkty.");
                            question.PointScore = pointScore;

                            // We have our question done, check if we were editing existing question or created new one
                            if (EditingQuestion != 0)
                                // We were editing existing question, replace old one with new one
                                Test.ReplaceQuestion(EditingQuestion, question);
                            else
                                // Its new question, simply add it to the test
                                Test.AddQuestion(question);

                            // Go to adding next question
                            SaveViewModelAndChangeToNewQuestion();
                        }
                        break;

                    default:
                        // Question type not found, don't submit any question
                        return;
                }

            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            // We have submitted question, reset the editing flag
            EditingQuestion = 0;
        }

        /// <summary>
        /// Gets back to previous question so user can edit it and submit it again
        /// </summary>
        private void EditQuestion(object param)
        {
            // Cast parameter to integer index
            var idx = (int)param;

            // Check if parameter is valid
            if (idx <= 0)
                return;

            // Indicate that we are now editing existing question
            EditingQuestion = idx;

            // Get this question from the list
            var baseQuestion = Test.Questions[idx-1];

            // Load data from this question (based on type of course)
            switch(baseQuestion.Type)
            {
                case QuestionType.MultipleChoice:
                    {
                        // Cast question to this type
                        var question = baseQuestion as MultipleChoiceQuestion;

                        // Set the combobox to this type
                        QuestionBeingAddedIndex = (int)QuestionType.MultipleChoice;

                        // Load data to the view model
                        QuestionTask = question.Task;
                        HowManyMultipleChoiceAnswersVisible = question.Options.Count;
                        AnswerA = question.Options[0];
                        AnswerB = question.Options[1];
                        if (HowManyMultipleChoiceAnswersVisible > 2)
                        {
                            ShouldAnswerCBeVisible = true;
                            AnswerC = question.Options[2];
                        }
                        if (HowManyMultipleChoiceAnswersVisible > 3)
                        {
                            ShouldAnswerDBeVisible = true;
                            AnswerD = question.Options[3];
                        }
                        if (HowManyMultipleChoiceAnswersVisible == 5)
                        {
                            ShouldAnswerEBeVisible = true;
                            AnswerE = question.Options[4];
                        }
                        RightAnswerIdx = question.CorrectAnswerIndex.ToString();
                        QuestionMultipleChoicePointScore = question.PointScore.ToString();
                    }
                    break;
                case QuestionType.MultipleCheckboxes:
                    {
                        // Cast question to this type
                        var question = baseQuestion as MultipleCheckboxesQuestion;

                        // Set the combobox to this type
                        QuestionBeingAddedIndex = (int)QuestionType.MultipleCheckboxes;

                        // Load data to the view model
                        QuestionTask = question.Task;
                        HowManyMultipleCheckboxesAnswersVisible = question.OptionsAndAnswers.Count;
                        Answer1 = question.OptionsAndAnswers.Keys.ElementAt(0);
                        IsAnswer1Right = question.OptionsAndAnswers.Values.ElementAt(0);
                        Answer2 = question.OptionsAndAnswers.Keys.ElementAt(1);
                        IsAnswer2Right = question.OptionsAndAnswers.Values.ElementAt(1);
                        if (HowManyMultipleCheckboxesAnswersVisible > 2)
                        {
                            ShouldAnswer3BeVisible = true;
                            Answer3 = question.OptionsAndAnswers.Keys.ElementAt(2);
                            IsAnswer3Right = question.OptionsAndAnswers.Values.ElementAt(2);
                        }
                        if (HowManyMultipleCheckboxesAnswersVisible > 3)
                        {
                            ShouldAnswer4BeVisible = true;
                            Answer4 = question.OptionsAndAnswers.Keys.ElementAt(3);
                            IsAnswer4Right = question.OptionsAndAnswers.Values.ElementAt(3);
                        }
                        if (HowManyMultipleCheckboxesAnswersVisible == 5)
                        {
                            ShouldAnswer5BeVisible = true;
                            Answer5 = question.OptionsAndAnswers.Keys.ElementAt(4);
                            IsAnswer5Right = question.OptionsAndAnswers.Values.ElementAt(4);
                        }
                        QuestionMultipleCheckboxesPointScore = question.PointScore.ToString();
                    }
                    break;
                case QuestionType.SingleTextBox:
                    {
                        // Cast question to this type
                        var question = baseQuestion as SingleTextBoxQuestion;

                        // Set the combobox to this type
                        QuestionBeingAddedIndex = (int)QuestionType.SingleTextBox;

                        // Load data to the view model
                        QuestionTask = question.Task;
                        TextBoxAnswer = question.CorrectAnswer;
                        QuestionSingleTextBoxPointScore = question.PointScore.ToString();
                    }
                    break;
                default:
                    // Something went wrong
                    Debugger.Break();
                    return;
            }
        }

        /// <summary>
        /// Cancels the editing mode and leads to brand new question
        /// </summary>
        private void CancelEditingQuestion()
        {
            // Ask the user if he wants to cancel editing
            var vm = new DecisionDialogViewModel
            {
                Title = "Anuluj edycję",
                Message = "Czy na pewno chcesz anulować edycję pytania?",
                AcceptText = "Tak",
                CancelText = "Nie"
            };
            IoCServer.UI.ShowMessage(vm);

            // If he declined, don't do anything
            if (!vm.UserResponse)
                return;

            // We are no more editing any question
            EditingQuestion = 0;

            // Go to brand new question
            SaveViewModelAndChangeToNewQuestion();
        }

        /// <summary>
        /// Deletes the question from test
        /// </summary>
        private void DeleteQuestion(object param)
        {
            // Cast parameter to integer index
            var idx = (int)param;

            // Ask the user if he wants to delete question
            var vm = new DecisionDialogViewModel
            {
                Title = "Usuwanie pytania",
                Message = "Czy na pewno chcesz usunąć pytanie numer " + idx.ToString() + "?",
                AcceptText = "Tak",
                CancelText = "Nie"
            };
            IoCServer.UI.ShowMessage(vm);

            // If he declined, don't do anything
            if (!vm.UserResponse)
                return;

            // Delete question from test with given index
            Test.RemoveQuestion(idx);

            // Reload questions from test to this view model
            ReloadQuestionsFromTest();
        }

        /// <summary>
        /// Selects pre-created criteria from the list and loads it into this view model
        /// </summary>
        /// <param name="param">Name of the selected criteria</param>
        private void SelectCriteria(object param)
        {
            // Cast parameter to string
            var criteriaName = param.ToString();

            // Search for the criteria with that name
            foreach (var criteria in CriteriaListViewModel.Instance.Items)
            {
                // Check if name matches
                if (criteria.Grading.Name == criteriaName)
                {
                    // Catch this grading object
                    CurrentGrading = criteria.Grading;

                    // Convert it to points
                    PointsGrading = CurrentGrading.ToPoints(Test.TotalPointScore);

                    // Set properties to match with current criteria
                    MatchPropertiesWithCriteria();
                }
            }
        }

        /// <summary>
        /// Leads into editing point criteria mode
        /// </summary>
        private void EditCriteria()
        {
            // Indicate that we are in editing mode
            IsCriteriaEditModeOn = true;

            // Set properties to match with current criteria
            MatchPropertiesWithCriteria();
        }

        /// <summary>
        /// Submits newly edited criteria
        /// </summary>
        private void SubmitCriteria()
        {
            try
            {
                // Check if input data is valid
                if (!ValidateInputData()) throw new Exception("Dane w polach są niepoprawne.");

                // Everything is ok, convert strings to integers and edit grading object
                if (PointsGrading.IsMarkAIncluded)
                {
                    PointsGrading.MarkA.TopLimit = int.Parse(EditingTopValueA);
                    PointsGrading.MarkA.BottomLimit = int.Parse(EditingBottomValueA);
                }
                PointsGrading.MarkB.TopLimit = int.Parse(EditingTopValueB);
                PointsGrading.MarkB.BottomLimit = int.Parse(EditingBottomValueB);
                PointsGrading.MarkC.TopLimit = int.Parse(EditingTopValueC);
                PointsGrading.MarkC.BottomLimit = int.Parse(EditingBottomValueC);
                PointsGrading.MarkD.TopLimit = int.Parse(EditingTopValueD);
                PointsGrading.MarkD.BottomLimit = int.Parse(EditingBottomValueD);
                PointsGrading.MarkE.TopLimit = int.Parse(EditingTopValueE);
                PointsGrading.MarkE.BottomLimit = int.Parse(EditingBottomValueE);
                PointsGrading.MarkF.TopLimit = int.Parse(EditingTopValueF);
                PointsGrading.MarkF.BottomLimit = int.Parse(EditingBottomValueF);

                // Get away from editing mode
                IsCriteriaEditModeOn = false;
                ErrorMessage = string.Empty;
                OnPropertyChanged(nameof(PointsGrading));
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Finally submits the whole test we've created
        /// </summary>
        private void SubmitTest()
        {
            // Check if data is correct
            if (!ValidateInputData())
                return;

            // Show a message box with info that we are submiting test
            IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
            {
                Title = "Test zapisany!",
                Message = "Tworzony test został zapisany.",
                OkText = "Ok"
            });

            // Attach criteria to the test
            Test.Grading = PointsGrading;
            
            try
            {
                // Try to save the test to file
                TestFileWriter.WriteToFile(Test);
            }
            catch (Exception ex)
            {
                // If an error occured, show info to the user
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Błąd zapisu",
                    Message = "Nie udało się zapisać tworzonego testu." +
                              "\nTreść błędu: " + ex.Message,
                    OkText = "Ok"
                });

                IoCServer.Logger.Log("Unable to save the test, error message: " + ex.Message);

                // Don't change the page
                return;
            }

            // Save this view model
            var viewModel = new TestEditorAddNewTestViewModel(Test);

            // Change page to result page
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorResult, viewModel);
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Gets the list of test's questions and then rewrites it to the questions collection in this VM
        /// </summary>
        public void ReloadQuestionsFromTest()
        {
            // Create new collection
            Questions = new ObservableCollection<Question>();

            // For each question in the test, add it to the collection
            foreach (var question in Test.Questions) Questions.Add(question);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Creates commands as we usually do in every constructor
        /// </summary>
        private void CreateCommands()
        {
            AddingQuestionsPageChangeCommand = new RelayCommand(ChangeQuestionsPage);
            AddingCriteriaPageChangeCommand = new RelayCommand(ChangeCriteriaPage);
            SubmitQuestionCommand = new RelayCommand(SubmitQuestion);
            SubmitTestCommand = new RelayCommand(SubmitTest);
            AddAnswerCommand = new RelayCommand(AddAnswer);
            RemoveAnswerCommand = new RelayCommand(RemoveAnswer);
            ChooseRightAnswerMultipleChoiceCommand = new RelayParameterizedCommand((param) => RightAnswerIdx = param.ToString());
            EditQuestionCommand = new RelayParameterizedCommand((param) => EditQuestion(param));
            CancelEditQuestionCommand = new RelayCommand(CancelEditingQuestion);
            DeleteQuestionCommand = new RelayParameterizedCommand((param) => DeleteQuestion(param));
            EditCriteriaCommand = new RelayParameterizedCommand((param) => SelectCriteria(param));
            EditPointsCriteriaCommand = new RelayCommand(EditCriteria);
            CancelEditPointsCommand = new RelayCommand(() => IsCriteriaEditModeOn = false);
            SubmitEditPointsCommand = new RelayCommand(SubmitCriteria);
        }

        /// <summary>
        /// Saves the current state of view model and changes page to the add new question page
        /// </summary>
        private void SaveViewModelAndChangeToNewQuestion()
        {
            // Save this view model
            var viewModel = new TestEditorAddNewTestViewModel(Test);

            // If user wants to set next question type as it was in previous one, set it
            if (IoCServer.Settings.IsNextQuestionTypeTheSame) viewModel.QuestionBeingAddedIndex = QuestionBeingAddedIndex;

            // Pass it to the next page
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorAddQuestions, viewModel);
        }

        /// <summary>
        /// Checks if criteria input data is valid
        /// </summary>
        private bool ValidateInputData()
        {
            try
            {
                // Parse every number value to integer
                var topMarkA = 0;
                var bottomMarkA = 0;
                if (PointsGrading.IsMarkAIncluded)
                {
                    topMarkA = int.Parse(EditingTopValueA);
                    bottomMarkA = int.Parse(EditingBottomValueA);
                }
                var topMarkB = int.Parse(EditingTopValueB);
                var bottomMarkB = int.Parse(EditingBottomValueB);
                var topMarkC = int.Parse(EditingTopValueC);
                var bottomMarkC = int.Parse(EditingBottomValueC);
                var topMarkD = int.Parse(EditingTopValueD);
                var bottomMarkD = int.Parse(EditingBottomValueD);
                var topMarkE = int.Parse(EditingTopValueE);
                var bottomMarkE = int.Parse(EditingBottomValueE);
                var topMarkF = int.Parse(EditingTopValueF);
                var bottomMarkF = int.Parse(EditingBottomValueF);

                // Check if input data is in sequence
                if (PointsGrading.IsMarkAIncluded)
                {
                    // The highest value must be test's max points
                    if (topMarkA != Test.TotalPointScore) throw new Exception();
                    if (bottomMarkA <= topMarkB) throw new Exception();
                }
                else
                {
                    // The highest value must be test's max points
                    if (topMarkB != Test.TotalPointScore) throw new Exception();
                }
                if (bottomMarkB <= topMarkC) throw new Exception();
                if (bottomMarkC <= topMarkD) throw new Exception();
                if (bottomMarkD <= topMarkE) throw new Exception();
                if (bottomMarkE <= topMarkF) throw new Exception();
                if (bottomMarkF != 0) throw new Exception();
            }
            catch
            {
                // Input data is invalid, show an error and return false
                ErrorMessage = "Dane w polach są niepoprawne.";
                return false;
            }

            // Everything works, return true
            return true;
        }

        /// <summary>
        /// Matches the string properties in this VM with actual PointsGrading values
        /// </summary>
        private void MatchPropertiesWithCriteria()
        {
            // Simply take values from PointsGrading to every property
            EditingTopValueA = PointsGrading.MarkA.TopLimit.ToString();
            EditingBottomValueA = PointsGrading.MarkA.BottomLimit.ToString();
            EditingTopValueB = PointsGrading.MarkB.TopLimit.ToString();
            EditingBottomValueB = PointsGrading.MarkB.BottomLimit.ToString();
            EditingTopValueC = PointsGrading.MarkC.TopLimit.ToString();
            EditingBottomValueC = PointsGrading.MarkC.BottomLimit.ToString();
            EditingTopValueD = PointsGrading.MarkD.TopLimit.ToString();
            EditingBottomValueD = PointsGrading.MarkD.BottomLimit.ToString();
            EditingTopValueE = PointsGrading.MarkE.TopLimit.ToString();
            EditingBottomValueE = PointsGrading.MarkE.BottomLimit.ToString();
            EditingTopValueF = PointsGrading.MarkF.TopLimit.ToString();
            EditingBottomValueF = PointsGrading.MarkF.BottomLimit.ToString();
        }

        #endregion
    }
}
