namespace Testinator.Core
{
    /// <summary>
    /// Package types as en enum
    /// </summary>
    public enum PackageType
    { 
        /// <summary>
        /// Arrival of this package means that client wants to disconnect or server is shutting down
        /// </summary>
        DisconnectRequest,
        
        /// <summary>
        /// The package contains information about the sender
        /// Used by the client to provide information about themselves
        /// </summary>
        Info,

        /// <summary>
        /// The package conatins current appliaction status
        /// Used by the clinet to report their state
        /// </summary>
        ReportStatus,

        /// <summary>
        /// The package conatins the test document 
        /// Used by the sever to send question paper to the client
        /// </summary>
        TestForm,

        /// <summary>
        /// The package contains the result form
        /// Used by the clinet to provide quiestion answers to the server
        /// </summary>
        ResultForm,

    }
}
