using WebInterface.HTTP;
using WebInterface.Response;
using WebInterface.Authorization;


namespace SmartHomeSystem.Websites
{
    class EncriptionExampleSite : IRequestInterface
    {
        public string Path { get; set; } = "/private/auth.json";
        public string Method { get; set; } = "";
        public int Port { get; set; } = 0;

        public HttpResponse OnRecieveRequest(HttpRequest request)
        {
            string tokenText = TokenManager.CreateToken(true, 800, "test");

            HttpResponse response = new HttpResponse("test");

            response.RedirectWithAuthorization("/private/secure.html",tokenText);

            return response;
        }
    }
}
