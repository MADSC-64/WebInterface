using WebInterface.HTTP;
using WebInterface.Response;
using WebInterface.IO;

namespace SmartHomeSystem.Websites
{
    class MainSite : IRequestInterface
    {
        public string Path { get; set; } = "/test*";
        public string Method { get; set; } = "";
        public int Port { get; set; } = 0;

        public HttpResponse OnRecieveRequest(HttpRequest request)
        {
            return FileManager.ReadFile(request);
        }
    }
}
