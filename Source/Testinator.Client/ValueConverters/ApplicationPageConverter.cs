using System.Diagnostics;
using Testinator.Client.Core;
using Testinator.UICore;

namespace Testinator.Client
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public static class ApplicationPageConverter
    {
        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and a view model, if any, and creates the desired page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            // Find the appropriate page
            switch (page)
            {
                case ApplicationPage.Login:
                    return new LoginPage(viewModel as LoginViewModel);

                case ApplicationPage.WaitingForTest:
                    return new WaitingForTestPage(viewModel as WaitingForTestViewModel);

                case ApplicationPage.ResultOverviewPage:
                    return new ResultOverviewPage(viewModel as ResultOverviewViewModel);

                case ApplicationPage.ResultQuestionsPage:
                    return new ResultQuestionsPage(viewModel as ResultQuestionsViewModel);

                #region Question Pages

                case ApplicationPage.QuestionMultipleCheckboxes:
                    return new QuestionMultipleCheckboxesPage(viewModel as QuestionMultipleCheckboxesViewModel);

                case ApplicationPage.QuestionMultipleChoice:
                    return new QuestionMultipleChoicePage(viewModel as QuestionMultipleChoiceViewModel);

                case ApplicationPage.QuestionSingleTextBox:
                    return new QuestionSingleTextBoxPage(viewModel as QuestionSingleTextBoxViewModel);

                #endregion

                default:
                    Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Find application page that matches the base page
            if (page is LoginPage)
                return ApplicationPage.Login;
            if (page is WaitingForTestPage)
                return ApplicationPage.WaitingForTest;
            if (page is ResultOverviewPage)
                return ApplicationPage.ResultOverviewPage;
            if (page is ResultQuestionsPage)
                return ApplicationPage.ResultQuestionsPage;
            if (page is QuestionMultipleChoicePage)
                return ApplicationPage.QuestionMultipleChoice;
            if (page is QuestionMultipleCheckboxesPage)
                return ApplicationPage.QuestionMultipleCheckboxes;
            if (page is QuestionSingleTextBoxPage)
                return ApplicationPage.QuestionSingleTextBox;

            // Alert developer of issue
            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
