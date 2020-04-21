using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RemoteFlix.Base.Helpers
{
    public static class NetworkHelper
    {
        public static string GetDefaultIp()
        {
            try
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    // This IP is arbitrary. We are sending an UDP
                    // package only to get the oubound IP address
                    socket.Connect("192.168.0.0", 1337);
                    
                    return (socket.LocalEndPoint as IPEndPoint)?.Address.ToString();
                }
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Classes.NetworkInterface> GetAvaliableNetworkInterfaces()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            var output = new List<Classes.NetworkInterface>();

            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ipInformation in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ipInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output.Add(new Classes.NetworkInterface(networkInterface.Name, ipInformation.Address));
                        }
                    }
                }
            }

            return output;
        }
    }
}