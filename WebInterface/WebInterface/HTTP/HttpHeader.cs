
namespace WebInterface.HTTP
{
    public class HttpHeader
    {
        public string name;
        public string value;

        public HttpHeader(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
