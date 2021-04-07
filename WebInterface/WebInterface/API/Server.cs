using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebInterface.API
{
    public class Server
    {
        public string serverName;
        public int port;

        TcpListener server;

        ServerSettings settings;

        public void Start(int port, int backlog, string name)
        {
            server = new TcpListener(IPAddress.Any, port);

            this.port = port;

            serverName = name;

            settings = new ServerSettings() { recieveTimeout = 3000, sendTimeot = 3000 };

            Console.WriteLine("Starting TCP Web Server Port:" + port);

            server.Start(backlog);

            Task.Run(ServerListener);
        }

        public void Start(int port, int backlog, string name, ServerSettings settings)
        {
            server = new TcpListener(IPAddress.Any, port);

            serverName = name;

            server.Start(backlog);

            this.settings = settings;

            Task.Run(ServerListener);
        }

        void ServerListener()
        {
            Console.WriteLine("Succesfuly Started Server");
            while (true)
            {
                IAsyncResult result = server.BeginAcceptTcpClient(ListenerCallback, server);

                result.AsyncWaitHandle.WaitOne();
            }
        }

        void ListenerCallback(IAsyncResult result)
        {
            TcpClient client = server.EndAcceptTcpClient(result);

            Task.Run(() => ClientHandler.HandleClient(client, this, settings));
        }

        public class ServerSettings
        {
            public int recieveTimeout;
            public int sendTimeot;


        }



    }
}
