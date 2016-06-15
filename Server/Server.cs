using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server : IDisposable
    {
        private const int clientDisconectedCheckFrequency = 10000;

        //private readonly NATUPNPLib.UPnPNAT _upnpTranslator = new NATUPNPLib.UPnPNAT();
        private readonly TcpListener tcpListener;

        private readonly List<TcpClient> tcpClients = new List<TcpClient>();

        private readonly int _maxClients;
        private readonly int _routerPort;
        private readonly int _localPort;             

        public Server(int maxClients, int routerPort, int localPort, string applicationName)
        {
            _maxClients = maxClients;
            _routerPort = routerPort;
            _localPort = localPort;

            //_upnpTranslator.StaticPortMappingCollection.Add(_routerPort, "TCP", _localPort, GetLocalIpAddress(), true, applicationName);

            tcpListener = new TcpListener(IPAddress.Any, _localPort);

            tcpListener.Start();


            //Accept clients
            new Thread(() =>
            {
                while (true)
                {
                    if (tcpClients.Count < _maxClients)
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        tcpClients.Add(client);

                        //new Thread(() =>
                        //{
                        //    HandleClient(client);

                        //}).Start();
                    }
                    else
                    {
                        Thread.Sleep(clientDisconectedCheckFrequency);
                    }
                }
            }).Start();
        }
        
        public void SendMessage(byte[] message)
        {
            foreach (TcpClient client in tcpClients)//Parallel foreach!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                new Thread(() =>
                {
                    client.Client.Send(message);
                }).Start();
            }
        }

        private void DisconnectClient(TcpClient tcpClient)
        {
            tcpClient.Close();
            tcpClients.Remove(tcpClient);
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
            tcpClients.ForEach(client => DisconnectClient(client));
            tcpListener.Stop();
            //_upnpTranslator.StaticPortMappingCollection.Remove(_routerPort, _protocol);
        }

    }
}
