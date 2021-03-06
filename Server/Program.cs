﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            NATUPNPLib.UPnPNAT _upnpTranslator = new NATUPNPLib.UPnPNAT();
            _upnpTranslator.StaticPortMappingCollection.Remove(45000, "UDP");

            //using (MulticastServer server = new MulticastServer(45000, "UDP", 45000, "My server"))
            //{
            //    while (true)
            //    {
            //        server.SendMessage("some message");
            //        System.Threading.Thread.Sleep(3000);
            //    }
            //}
        }
    }

    class MulticastServer : IDisposable
    {
        private readonly NATUPNPLib.UPnPNAT _upnpTranslator = new NATUPNPLib.UPnPNAT();
        private readonly UdpClient udpSender;

        private readonly IPAddress localIpAddress;

        private readonly int _externalPort;
        private readonly int _internalPort;
        private readonly string _protocol;    

        public MulticastServer(int externalPort, string protocol, int internalPort, string applicationName)
        {
            _externalPort = externalPort;
            _internalPort = internalPort;
            _protocol = protocol;

            localIpAddress = GetLocalIpAddress();
            
            _upnpTranslator.StaticPortMappingCollection.Add(_externalPort, _protocol, _internalPort, localIpAddress.ToString(), true, applicationName);
            Console.WriteLine(localIpAddress.ToString());
            //IPEndPoint ipEndPoint = new IPEndPoint(localIpAddress, _internalPort);
            //IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("239.192.100.2"), _internalPort);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("31.43.129.211"), _internalPort);
            udpSender = new UdpClient();

            //udpSender.AllowNatTraversal(true);


            udpSender.Connect(ipEndPoint);
        }

        public void SendMessage(string message)
        {
            udpSender.Send(Encoding.UTF8.GetBytes(message), message.Length); //////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! pass full msg
        }

        private IPAddress GetLocalIpAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }
            }
            throw new Exception("Local IP address was not found");
        }


        void IDisposable.Dispose()
        {
            udpSender.Close();
            _upnpTranslator.StaticPortMappingCollection.Remove(_externalPort, _protocol);
        }
        
    }
}
