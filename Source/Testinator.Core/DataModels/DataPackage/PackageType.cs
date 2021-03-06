﻿namespace Testinator.Core
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

        /// <summary>
        /// The package indicates the start of a test and contains startup arguments
        /// </summary>
        BeginTest,

        /// <summary>
        /// The package indicates that the user is ready to start the test
        /// </summary>
        ReadyForTest,

        /// <summary>
        /// Sent by the server to stop the test forcefully
        /// Package content should be null in this context
        /// </summary>
        StopTestForcefully,
    }
}