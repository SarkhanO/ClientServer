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

            ReceiveSendMessages(client);

        }

        private static void ReceiveSendMessages(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                int messageBufferSize = 256;
                byte[] messageBuffer = new byte[messageBufferSize];

                new Thread(() =>
                {
                    while (true)
                    {
                        StringBuilder message = new StringBuilder();
                        int receivedBytes = 0;
                        do
                        {
                            receivedBytes = stream.Read(messageBuffer, 0, messageBufferSize);
                            message.Append(Encoding.UTF8.GetString(messageBuffer), 0, receivedBytes);
                        }
                        while (receivedBytes == messageBufferSize);
                    }

                }).Start();

                while (true)
                {
                    string message = Console.ReadLine();
                    stream.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
                }

            }
        }
    }
}
