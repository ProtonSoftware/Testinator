using System;

namespace Testinator.Core
{
    /// <summary>
    /// A client model that is saved with the results
    /// It's a light version of <see cref="ClientModel"/> is it doesn't take much disk space
    /// </summary>
    [Serializable]
    public class TestResultsClientModel : Client
    {
        /// <summary>
        /// Points scored by the user
        /// </summary>
        public int PointsScored { get; set; }

        /// <summary>
        /// The client mark
        /// </summary>
        public Marks Mark { get; set; }

        #region Constructor

        /// <summary>
        /// Constructs the results model
        /// </summary>
        /// <param name="model">The client this results are from</param>
        public TestResultsClientModel(Client model)
        {
            Name = model.Name;
            LastName = model.LastName;
            MachineName = model.MachineName;
        }

        public TestResultsClientModel()
        {

        }

        #endregion
    }
}
