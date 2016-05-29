using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Server server = new Server(8, 45000, "TCP", 45000, "My server"))
            {
                new Thread(() =>
                {
                    while(true)
                    {
                        Console.WriteLine(server.GetNextCleintMessage());
                    }
                }).Start();
                while(true)
                {
                    server.SendMessage(Console.ReadLine());
                }
            }
        }
    }

    class Server : IDisposable
    {
        private const int clientDisconectedCheckFrequency = 10000;
        private const int messageBufferSize = 256;

        private readonly NATUPNPLib.UPnPNAT _upnpTranslator = new NATUPNPLib.UPnPNAT();
        private readonly TcpListener tcpListener;

        private readonly List<TcpClient> tcpClients = new List<TcpClient>();
        private readonly Stack<string> receivedMessages = new Stack<string>();
        
        private readonly int _maxClients;
        private readonly int _routerPort;
        private readonly int _localPort;
        private readonly string _protocol;    

        public Server(int maxClients, int routerPort, string protocol, int localPort, string applicationName)
        {
            _maxClients = maxClients;
            _routerPort = routerPort;
            _localPort = localPort;
            _protocol = protocol;
            
            //_upnpTranslator.StaticPortMappingCollection.Add(_routerPort, _protocol, _localPort, GetLocalIpAddress(), true, applicationName);

            tcpListener = new TcpListener(IPAddress.Any, _localPort);
            tcpListener.Start();


            //Accept clients
            new Thread(() =>
            {
                while(true)
                {
                    if (tcpClients.Count < _maxClients)
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        tcpClients.Add(client);

                        new Thread(() =>
                        {
                            HandleClient(client);

                        }).Start();
                    }
                    else
                    {
                        Thread.Sleep(clientDisconectedCheckFrequency);
                    }
                }
            }).Start();            
        }

        /// <summary>
        /// Sends message to all clients
        /// </summary>
        /// <param name="message">Message to send</param>
        public void SendMessage(string message)
        {
            SendMessage(tcpClients, message);
        }

        public string GetNextCleintMessage()
        {
            return receivedMessages.Pop();
        }

        private void HandleClient(TcpClient client)
        {
            byte[] messageBuffer = new byte[messageBufferSize];

            while (client.Connected)
            {
                StringBuilder message = new StringBuilder();
                int receivedBytes = 0;
                do
                {
                    receivedBytes = client.Client.Receive(messageBuffer);
                    message.Append(Encoding.UTF8.GetString(messageBuffer), 0, receivedBytes);
                }
                while (receivedBytes == messageBufferSize);
                receivedMessages.Push(message.ToString());

                //Redirect received message to other clients
                SendMessage(tcpClients.Except(new List<TcpClient> { client }), message.ToString());
            }

            client.Close();
            tcpClients.Remove(client);
        }

        /// <summary>
        /// Sends message to specific clients
        /// </summary>
        /// <param name="clients">List of clients to send a message</param>
        /// <param name="message">Message to send</param>
        private void SendMessage(IEnumerable<TcpClient> clients, string message)
        {
            foreach (TcpClient client in tcpClients)
            {
                new Thread(() =>
                {
                    client.Client.Send(Encoding.UTF8.GetBytes(message));
                }).Start();
            }
        }

        private string GetLocalIpAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress.ToString();
                }
            }
            throw new Exception("Local IP address was not found");
        }
        
        void IDisposable.Dispose()
        {
            tcpClients.ForEach(client => client.Close());
            tcpListener.Stop();
            //_upnpTranslator.StaticPortMappingCollection.Remove(_routerPort, _protocol);
        }
        
    }
}
