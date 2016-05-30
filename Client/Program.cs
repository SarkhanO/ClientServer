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
            TcpClient client = new TcpClient();
            Console.WriteLine("Enter ip address");
            client.Connect(IPAddress.Parse(Console.ReadLine()), 45000);

            int messageBufferSize = 256;

            new Thread(() =>
            {
                byte[] messageBuffer = new byte[messageBufferSize];

                while (true)
                {
                    StringBuilder message = new StringBuilder();
                    int receivedBytes = 0;
                    do
                    {
                        receivedBytes = client.Client.Receive(messageBuffer);
                        message.Append(Encoding.UTF8.GetString(messageBuffer), 0, receivedBytes);
                    }
                    while (receivedBytes == messageBufferSize);
                    Console.WriteLine(message.ToString());
                }
            }).Start();


            //while(true)
            //{
            //    client.Client.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            //}

        }
    }
}
