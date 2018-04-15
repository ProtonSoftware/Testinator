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
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleChoice, questionViewModel);
                        break;
                    }

                case QuestionType.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(questionToShow as MultipleCheckBoxesQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel);
                        break;
                    }

                case QuestionType.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(questionToShow as SingleTextBoxQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionSingleTextBox, questionViewModel);
                        break;
                    }
            }
        }
    }
}
