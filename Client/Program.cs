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
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 45000);
            byte[] msg = new byte[256];
            tcpClient.Client.Receive(msg);
            Console.WriteLine(Encoding.UTF8.GetString(msg));
            Console.Read();
        }
    }
}
