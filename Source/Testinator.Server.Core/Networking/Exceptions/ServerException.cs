using System;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The server side Exception implementation
    /// </summary>
    public class ServerException : Exception
    {
        #region Private Properties

        /// <summary>
        /// Defines if the <see cref="Reason"/> string comes from innerexception's message or directly from this class
        /// </summary>
        private bool UseInnerException { get; set; }

        /// <summary>
        /// The reason of this exception
        /// </summary>
        private string mReason { get; set; } = "";

        #endregion

        #region Public Properties

        /// <summary>
        /// The reason of this exception
        /// </summary>
        public string Reason => InnerException == null ? mReason : InnerException.Message;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Message">The message of this exception</param>
        public ServerException(string Message) : base(Message) { }

        /// <summary>
        /// Constructs exepction with inner exception 
        /// </summary>
        /// <param name="Message">The message of this exception</param>
        /// <param name="InnerException">The exception that caused the problem</param>
        public ServerException(string Message, Exception InnerException) : base(Message, InnerException)
        {
            UseInnerException = true;
        }

        /// <summary>
        /// Constructs exception with the message string and a resson string 
        /// </summary>
        /// <param name="Message">The main message</param>
        /// <param name="Reason">The probable reason of this exception</param>
        public ServerException(string Message, string Reason) : base(Message)
        {
            mReason = Reason;
        }

        #endregion
    }
}
