using System.Net;
using System.Net.Sockets;

namespace Testinator.Core
{ 
    /// <summary>
    /// Helper class for managing IP addresses
    /// </summary>
    public static class IpHelpers
    {
        /// <summary>
        /// Gets machine local ip address
        /// </summary>
        /// <returns>Ip address as a string</returns>
        public static string GetLocalIPAddressString()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets machine local ip address 
        /// </summary>
        /// <returns>Local machine address as a <see cref="IPAddress"/></returns>
        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return IPAddress.Parse("127.0.0.1");
        }
    }
}
