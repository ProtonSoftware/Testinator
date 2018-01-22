namespace Testinator.Core
{
    /// <summary>
    /// List of every saveable object which can be written to the file
    /// </summary>
    public enum SaveableObjects
    {
        /// <summary>
        /// Default object
        /// </summary>
        None,

        /// <summary>
        /// The test file (binary)
        /// </summary>
        Test,

        /// <summary>
        /// The grading file (xml)
        /// </summary>
        Grading,

        /// <summary>
        /// The test results file (binary)
        /// </summary>
        Results,

        /// <summary>
        /// The application config file with settings inside (xml)
        /// </summary>
        Config
    }
}
