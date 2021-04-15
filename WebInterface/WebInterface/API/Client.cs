using System;
using System.Net.Sockets;
using WebInterface.HTTP;

namespace WebInterface.API
{
    public class Client
    {

        public static HttpResponse GetHttpResponse(string ip, int port, HttpRequest request)
        {
            TcpClient client = new TcpClient();

            try
            {
                client.Connect(ip, port);

                NetworkStream stream = client.GetStream();

                request.WriteToStream(stream);

                var response = HttpResponse.FromStream(stream);

                return response;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
