using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RemoteFlix.Base.Helpers
{
    public static class NetworkHelper
    {
        public static IPAddress GetLocalIPAddress()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
