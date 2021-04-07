using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInterface.HTTP;
using WebInterface.Response;

namespace SmartHomeSystem.Websites
{
    class EntrySite : IRequestInterface
    {
        public string Path { get; set; } = "/";
        public string Method { get; set; } = "";

        public int Port { get; set; } = 0;

        public HttpResponse OnRecieveRequest(HttpRequest request)
        {
            HttpResponse response = new HttpResponse("test");

            response.Redirect("test/index.html");

            Console.WriteLine("response");

            return response;
        }
    }
}
