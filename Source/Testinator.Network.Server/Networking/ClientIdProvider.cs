using System.Collections.Generic;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Provides unique IDs for clients
    /// </summary>
    public static class ClientIdProvider
    {
        #region Private Members

        /// <summary>
        /// Keeps track of id's given to each user
        /// </summary>
        private static Dictionary<string, string> MacId = new Dictionary<string, string>();

        /// <summary>
        /// Start at ID = 1
        /// </summary>
        private static int StartNumber = 1;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets id for a client
        /// </summary>
        /// <param name="MacAddress">Mac address of the client, used to check is the client has an Id assigned to them</param>
        /// <returns>The id</returns>
        public static string GetId(string MacAddress)
        {
            // TODO: need to remake this 
            // If the user is known to the server don't make new id for them
            //if (MacId.TryGetValue(MacAddress, out string Id))
            //    return Id;

            // If the user it not known get them new id
            var Id = "#" + StartNumber.ToString();
            StartNumber++;

            // Store the id
            //MacId.Add(MacAddress, Id);

            return Id;
        }

        #endregion

    }
}