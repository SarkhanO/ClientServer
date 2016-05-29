using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();
            Console.WriteLine("Enter ip address");
            tcpClient.Connect(IPAddress.Parse(Console.ReadLine()), 45000);
            byte[] msg = new byte[256];

            while (true)
            {
                StringBuilder message = new StringBuilder();
                int receivedBytes = 0;
                do
                {
                    receivedBytes = tcpClient.Client.Receive(msg);
                    message.Append(Encoding.UTF8.GetString(msg), 0, receivedBytes);
                }
                while (receivedBytes == 256);
                Console.WriteLine(message.ToString());
                Console.WriteLine();
                }
            }

        class Client
        {

        }
    }
}
