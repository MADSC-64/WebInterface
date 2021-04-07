using System.Collections.Generic;

namespace WebInterface.HTTP
{
    class HttpResponseCodes
    {
        static readonly Dictionary<int, string> responseCodes = new Dictionary<int, string>()
        {
            {200,"OK" },
            {301, "Moved Permanently" },
            {400, "Bad Request"},
            {401, "Unauthorized"},
            {404,"Not Found" },
            {500, "Internal Server Error" },
        };

        public static string ResponseCodeText(int responseCode)
        {
            if (responseCodes.ContainsKey(responseCode))
                return responseCodes[responseCode];

            return "";
        }
    }
}
