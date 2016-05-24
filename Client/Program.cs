using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter IP address");
            //IPAddress serverIpAddress = IPAddress.Parse(Console.ReadLine());
            IPAddress serverIpAddress = IPAddress.Parse("239.192.100.2");
            IPEndPoint serverIpEndPoint = new IPEndPoint(serverIpAddress, 45000);
            //UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Parse("192.168.1.102"), 45001));
            UdpClient client = new UdpClient(45001);

            //client.AllowNatTraversal(true);
            client.Connect(serverIpEndPoint);
            
            //UdpClient client = new UdpClient();

            //IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.54"), 45000);
            client.JoinMulticastGroup(serverIpAddress);

            IPEndPoint senderIpEndPoint = null;
            Console.WriteLine(Encoding.UTF8.GetString(client.Receive(ref senderIpEndPoint)));
            Console.Read();
        }
    }
}
