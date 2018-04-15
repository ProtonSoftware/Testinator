using System;
using System.Net;
using System.Net.Sockets;

namespace Testinator.Core
{
    /// <summary>
    /// Helper class for managing IP addresses
    /// </summary>
    public static class NetworkHelpers
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

        /// <summary>
        /// Chcecks if the address if a correct ip address
        /// </summary>
        /// <param name="ip">The address to chceck</param>
        /// <returns>True if address is correct, false if not</returns>
        public static bool IsIPAddressCorrect(string ip)
        {
            return IPAddress.TryParse(ip, out var ipa);
        }

        /// <summary>
        /// Checks if the string contains valid srver port
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsPortCorrect(string port)
        {
            if (!int.TryParse(port, out var mport))
                return false;

            if (mport < 0 || mport > 9999)
                return false;

            return true;
        }
    }
}