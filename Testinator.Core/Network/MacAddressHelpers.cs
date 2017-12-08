using System.Net.NetworkInformation;

namespace Testinator.Core.Network
{
    /// <summary>
    /// Help with mac addresses
    /// </summary>
    public static class MacAddressHelpers
    {
        public static string GetMac()
        {
            string macAddress = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
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