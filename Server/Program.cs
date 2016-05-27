﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Server server = new Server(8, 45000, "TCP", 45000, "My server"))
            {
                while (true)
                {
                    server.SendMessage("some message");
                    System.Threading.Thread.Sleep(3000);
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

        private readonly List<TcpClient> tcpClients;
        
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
            ThreadPool.SetMaxThreads(_maxClients, _maxClients);
            tcpClients = new List<TcpClient>();
            new Thread(() =>
            {
                while(true)
                {
                    if (tcpClients.Count < _maxClients)
                    {
                        tcpClients.Add(tcpListener.AcceptTcpClient());
                    }

                    tcpClients.ForEach((client) =>
                    {
                        if (!client.Connected)
                        {
                            tcpClients.Remove(client);
                        }
                    });
                    Thread.Sleep(clientDisconectedCheckFrequency);
                }
            }
            ).Start();
        }

        public void SendMessage(string message)
        {
            foreach(TcpClient client in tcpClients)
            {
                ThreadPool.QueueUserWorkItem((obj) => 
                {
                    client.Client.Send(Encoding.UTF8.GetBytes(message));
                });
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
            tcpListener.Stop();
            //_upnpTranslator.StaticPortMappingCollection.Remove(_routerPort, _protocol);
        }
        
    }
}
