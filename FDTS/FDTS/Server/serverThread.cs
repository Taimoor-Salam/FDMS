using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
namespace FDMS.Server
{
    class serverThread
    {
        public void startServerThread()
        {
            ThreadStart st = new ThreadStart(runServer);
            Thread serverThread = new Thread(st);
            serverThread.Start();
        }

        public void runServer()
        {
            UDPServer server = new UDPServer();
            server.Receive();
        }
    }
}
