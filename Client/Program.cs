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
            Console.WriteLine("Enter IP address");
            IPAddress serverIpAddress = IPAddress.Parse(Console.ReadLine());
            IPEndPoint serverIpEndPoint = new IPEndPoint(serverIpAddress, 45000);
            UdpClient client = new UdpClient(serverIpEndPoint);


            
            //IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.54"), 45000);
            //client.JoinMulticastGroup(IPAddress.Parse("192.168.1.100"));

            IPEndPoint senderIpEndPoint = null;
            Console.WriteLine(Encoding.UTF8.GetString(client.Receive(ref senderIpEndPoint)));
            Console.Read();
        }
    }
}
