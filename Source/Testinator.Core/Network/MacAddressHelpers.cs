using System.Net.NetworkInformation;

namespace Testinator.Core.Network
{
    /// <summary>
    /// Helps with mac addresses
    /// </summary>
    public static class MacAddressHelpers
    {
        /// <summary>
        /// Gets the machine mac address
        /// </summary>
        /// <returns>This machine mac address</returns>
        public static string GetMac()
        {
            var macAddress = string.Empty;
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                //if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddress;
        }
    }
}