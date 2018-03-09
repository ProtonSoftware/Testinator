namespace Testinator.Updater
{
    /// <summary>
    /// Every page in this application as an enum
    /// </summary>
    public enum ApplicationPage
    {
        /// <summary>
        /// No page
        /// </summary>
        None = 0,

        /// <summary>
        /// The initial page
        /// </summary>
        Initial,

        /// <summary>
        /// The update downloading page
        /// </summary>
        Download,

        /// <summary>
        /// The exit page shown when update is done
        /// </summary>
        Exit
    }
}
