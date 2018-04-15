namespace Testinator.Server.Core
{
    /// <summary>
    /// Represents options for test startup
    /// </summary>
    public class TestOptions
    {
        /// <summary>
        /// Indicated if full screen is allowed
        /// </summary>
        public bool FullScreenEnabled { get; set; }

        /// <summary>
        /// Indicates if results page is allowed
        /// </summary>
        public bool ResultsPageAllowed { get; set; }
    }
}
