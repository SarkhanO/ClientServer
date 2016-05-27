using System;
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
            using (Server server = new Server(45000, "TCP", 45000, "My server"))
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
        private readonly NATUPNPLib.UPnPNAT _upnpTranslator = new NATUPNPLib.UPnPNAT();
        private readonly TcpListener tcpListener;

        private readonly int _externalPort;
        private readonly int _internalPort;
        private readonly string _protocol;    

        public Server(int externalPort, string protocol, int internalPort, string applicationName)
        {
            _externalPort = externalPort;
            _internalPort = internalPort;
            _protocol = protocol;
            
            _upnpTranslator.StaticPortMappingCollection.Add(_externalPort, _protocol, _internalPort, GetLocalIpAddress(), true, applicationName);

            tcpListener = new TcpListener(IPAddress.Any, 45000);

             tcpListener.AcceptTcpClient()
        }

        public void SendMessage(string message)
        {
            tcpListener.Server.Send(Encoding.UTF8.GetBytes(message)); //////////////////////////////////////!!!!!!!!!!!
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
            _upnpTranslator.StaticPortMappingCollection.Remove(_externalPort, _protocol);
        }
        
    }
}
