using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Helper methods for questions
    /// </summary>
    public static class QuestionHelpers
    {
        /// <summary>
        /// Changes page to the questions corresponding page
        /// </summary>
        /// <param name="questionToShow"></param>
        public static void ShowQuestion(Question questionToShow)
        {
            // Based on next question type...
            switch (questionToShow.Type)
            {
                case QuestionType.MultipleChoice:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleChoiceViewModel();
                        questionViewModel.AttachQuestion(questionToShow as MultipleChoiceQuestion);
                        IoCClient.UI.DispatcherThreadAction(() => IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleChoice, questionViewModel));
                        break;
                    }

                case QuestionType.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(questionToShow as MultipleCheckBoxesQuestion);
                        IoCClient.UI.DispatcherThreadAction(() => IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel));
                        break;
                    }

                case QuestionType.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(questionToShow as SingleTextBoxQuestion);
                        IoCClient.UI.DispatcherThreadAction(() => IoCClient.Application.GoToPage(ApplicationPage.QuestionSingleTextBox, questionViewModel));
                        break;
                    }
            }
        }

        /// <summary>
        /// Gets view data for restuls page from a question
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <param name="Index">Index in the list</param>
        /// <returns></returns>
        public static BaseViewModel ToViewModel(this Question question, Answer answer, int Index)
        {
            switch(question.Type)
            {
                case QuestionType.MultipleChoice:
                {
                    var questionViewModel = new QuestionMultipleChoiceViewModel
                    {
                        UserAnswer = (MultipleChoiceAnswer)answer,
                        Index = Index,
                    };
                    questionViewModel.AttachQuestion(question as MultipleChoiceQuestion, true);
                    return questionViewModel;
                }

                case QuestionType.MultipleCheckboxes:
                {
                    var questionViewModel = new QuestionMultipleCheckboxesViewModel
                    {
                        UserAnswer = (MultipleCheckBoxesAnswer)answer,
                        Index = Index,
                    };
                    questionViewModel.AttachQuestion(question as MultipleCheckBoxesQuestion, true);
                    return questionViewModel;
                }

                case QuestionType.SingleTextBox:
                {
                    var questionViewModel = new QuestionSingleTextBoxViewModel
                    {
                        UserAnswer = (SingleTextBoxAnswer)answer,
                        Index = Index,
                    };
                    questionViewModel.AttachQuestion(question as SingleTextBoxQuestion, true);
                    return questionViewModel;
                }
                default:
                    return null;
            }
            
        }
    }
}
