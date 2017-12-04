using System.Net;
using System.Net.Sockets;

namespace Testinator.Core
{
    /// <summary>
    /// Helpers for manipulating <see cref="Socket"/>s
    /// </summary>
    public static class SocketHelpers
    {
        /// <summary>
        /// Gets ip address from a socket
        /// </summary>
        /// <param name="source">Source socket</param>
        /// <returns>Ip address as a string</returns>
        public static string GetIp(this Socket source)
        {
            if (source == null)
                return string.Empty;

            return ((IPEndPoint)source.RemoteEndPoint).Address.ToString();
        }
    }
}