using System;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Defines the structure of the client connected to the sever
    /// </summary>
    public class ClientModel : Client
    {
        #region Public Properties

        /// <summary>
        /// Indicates if the user can start the test, meaning: is not in the result page but in the waiting for test page
        /// </summary>
        public bool CanStartTest { get; set; } = true;

        /// <summary>
        /// Number of questions this client have done so far
        /// </summary>
        public int CurrentQuestion { get; set; }

        /// <summary>
        /// The number of qustions in the test,
        /// makes sense only if the test in progress 
        /// </summary>
        public int QuestionsCount { get; set; }

        /// <summary>
        /// The value for the progress bar, substract one from the CurrentQuestion to show the progress correctly
        /// </summary>
        public int ProgressBarValue => CurrentQuestion - 1;

        /// <summary>
        /// The percentage value for the progress bar;
        /// </summary>
        public string ProgressBarPercentage => Math.Floor((double)ProgressBarValue / QuestionsCount * 100).ToString() + "%";

        /// <summary>
        /// Indicates if there is any connection problems with this client
        /// </summary>
        public bool ConnectionProblem { get; set; }

        /// <summary>
        /// The answer given by this user
        /// </summary>
        public List<Answer> Answers { get; set; }

        /// <summary>
        /// Points scored by the user
        /// </summary>
        public int PointsScored { get; set; }

        /// <summary>
        /// The client mark
        /// </summary>
        public Marks Mark { get; set; }

        #endregion

    }
}