using System.Net;

namespace RemoteFlix.Base.Classes
{
    public class NetworkInterface
    {
        public string Name { get; }
        public IPAddress InterNetworkAddress { get; }

        public NetworkInterface(string name, IPAddress interNetworkAddress)
        {
            Name = name;
            InterNetworkAddress = interNetworkAddress;
        }

        public override string ToString()
        {
            return $"{InterNetworkAddress.ToString()} ({Name})";
        }
    }
}
