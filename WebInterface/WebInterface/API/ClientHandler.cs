using System;
using System.Net.Sockets;
using WebInterface.Response;

namespace WebInterface.API
{
    class ClientHandler
    {



        public static void HandleClient(TcpClient client, Server server, Server.ServerSettings settings)
        {
            try
            {
                using NetworkStream stream = client.GetStream();

                //client.SendTimeout = settings.sendTimeot;
                //client.ReceiveTimeout = settings.recieveTimeout;

                HTTP.HttpRequest request = HTTP.HttpRequest.FromStream(stream);

                var response = ResponseManager.GenerateResponse(request, server);

                response.server = server.serverName;

                response.WriteToStream(stream);

                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Client Handling Error:" + e);
            }

        }
    }


}
