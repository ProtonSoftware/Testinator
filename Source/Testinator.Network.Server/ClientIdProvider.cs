namespace Testinator.Network.Server
{
    /// <summary>
    /// Provides unique ID for clients
    /// </summary>
    public static class ClientIdProvider
    {
        /// <summary>
        /// Start at ID = 1
        /// </summary>
        private static int StartNumber = 1;

        public static string GetId()
        {
            var id = "#" + StartNumber.ToString();
            StartNumber++;
            return id;
        }

    }
}
