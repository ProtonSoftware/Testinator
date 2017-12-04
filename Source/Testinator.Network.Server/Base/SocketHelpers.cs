using System.Net.Sockets;
using Testinator.Core;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Provides helper methods for <see cref="Socket"/>
    /// </summary>
    public static class SocketHelpers
    {
        /// <summary>
        /// Sends a data package to the client
        /// </summary>
        /// <param name="target">Target socket</param>
        /// <param name="data">Data to be sent</param>
        public static void SendPackage(this Socket target, DataPackage data)
        {
            // If not possible to convert object return
            if (!DataPackageDescriptor.TryConvertToBin(out byte[] sendBuffor, data))
                return;

            target.Send(sendBuffor, 0, sendBuffor.Length, SocketFlags.None);
        }
    }
}
