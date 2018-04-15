namespace Testinator.Server
{
    /// <summary>
    /// Describes oprations that a test editor can do
    /// IMPORTANT: order is crucial, dont change it! (see TestEditor for more info)
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// Nothing is being done
        /// </summary>
        None,

        /// <summary>
        /// Represents the process of editing basic test information
        /// </summary>
        EditingInformation,

        /// <summary>
        /// Represents the process of editing questions
        /// </summary>
        EditingQuestions,

        /// <summary>
        /// Represents the process of editing criteria
        /// </summary>
        EditingCriteria,

        /// <summary>
        /// Represents the process of finalizing test edition
        /// </summary>
        Finalizing,
    }
}
