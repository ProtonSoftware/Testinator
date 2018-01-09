namespace Testinator.Core
{
    /// <summary>
    /// Extension methods for <see cref="Test"/> class
    /// </summary>
    public static class TestExtensionMethods
    {
        #region Public Methods

        /// <summary>
        /// Adds question and correct answer to it to the test
        /// </summary>
        /// <param name="question">Question to be added</param>
        /// <param name="answer">Correct answer to it</param>
        public static void AddQuestion(this Test test, Question question)
        {
            // Don't add null items
            if (question == null)
                return;

            // Get a copy of the question because questions need to be unique, 
            // if this same question is added multiple times index is changed in all instances and we end up having mulitple questions with id eg. 5
            var questionCopy = question.Clone();

            // Id for this question
            var id = test.Questions.Count + 1;
            questionCopy.ID = id;

            // Save them
            test.Questions.Add(questionCopy);
        }

        /// <summary>
        /// Removes question from the test
        /// </summary>
        /// <param name="question">The index(id) of the question to be removed</param>
        public static void RemoveQuestion(this Test test, int index)
        {
            if (index <= 0 || index > test.Questions.Count)
                return;

            // Remove the question
            test.Questions.RemoveAt(index - 1);

            // Update all indexes that are on the right from the deleted question, -1 because we strat indexing at 1 but list is indexed from 0
            while (index <= test.Questions.Count)
            {
                test.Questions[index - 1].ID = index;
                index++;
            }
        }

        /// <summary>
        /// Replaces question at the given index
        /// </summary>
        /// <param name="test"></param>
        /// <param name="index">The index to be updated</param>
        /// <param name="NewQuestion">New Question to be stored</param>
        public static void ReplaceQuestion(this Test test, int index, Question NewQuestion)
        {
            if (index <= 0 || index > test.Questions.Count || NewQuestion == null)
                return;

            // Get a copy of the new question
            var questionCopy = NewQuestion.Clone();

            // Copy the id from the existing question
            NewQuestion.ID = test.Questions[index - 1].ID;

            // Replace the question
            test.Questions[index - 1] = NewQuestion;

        }

        /// <summary>
        /// Removes all questions and answers from this test
        /// </summary>
        public static void ClearQuestions(this Test test)
        {
            test.Questions.Clear();
        }

        /// <summary>
        /// Get maximum possible score from the test
        /// </summary>
        /// <param name="test"></param>
        /// <returns>Max possible score</returns>
        public static int MaxPossibleScore(this Test test)
        {
            int max = 0;
            foreach (var q in test.Questions)
                max += q.PointScore;

            return max;
        }

        #endregion
    }
}
