using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ClientDisconnectedException : Exception
    {
        public ClientDisconnectedException(string message = null) : base(message)
        {
        }
    }
}
