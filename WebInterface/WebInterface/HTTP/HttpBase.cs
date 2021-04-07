using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace WebInterface.HTTP
{
    public abstract class HttpBase
    {
        public string server;

        public Dictionary<string, string> headers = new Dictionary<string, string>();

        public Dictionary<string, string> queryParameters = new Dictionary<string, string>();

        //---------------------Encoding---------------------------

        //Converts All Headers To A Single String For Easier Writing To Stream
        public string HeadersToString()
        {
            string headerText = "";
            if (headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    headerText += header.Key + ":" + header.Value + "\n";
                }
            }

            return headerText;
        }

        //Converts All Queries To A Single String For Easier Writing To Stream
        public string QueryToString()
        {
            string headerText = "";
            if (queryParameters.Count > 0)
            {
                headerText = "?";
                foreach (KeyValuePair<string, string> header in queryParameters)
                {
                    headerText += header.Key + "=" + header.Value + "&";
                }

                headerText = headerText.Remove(headerText.Length - 1, 1);
            }

            return headerText;
        }

        //Writes Header Data To Stream
        public static void WriteHttpHeadersToStream(string httpData, Stream stream)
        {
            //Write Http Data To Stream
            byte[] httpBytes = Encoding.UTF8.GetBytes(httpData);
            stream.Write(httpBytes, 0, httpBytes.Length);
        }

        //---------------------Decoding---------------------------

        //Gets Http Header From Stream Reader
        public static Dictionary<string, string> GetHeaders(StreamReader reader)
        {
            string line;

            Dictionary<string, string> headers = new Dictionary<string, string>();

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Equals("")) return headers;

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                string name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line[pos..];
                headers[name] = value;
            }

            return headers;
        }

        //Gets Query Parameters From Url
        public static Dictionary<string, string> GetQueryParameters(Uri url)
        {
            string query = url.Query;

            Dictionary<string, string> queryParameters = new Dictionary<string, string>();

            if (query.Length > 0)
            {

                var parsedQuery = HttpUtility.ParseQueryString(query);

                Console.WriteLine(parsedQuery.AllKeys.Length);

                foreach (var key in parsedQuery.AllKeys)
                {
                    queryParameters.Add(key, parsedQuery[key]);
                    Console.WriteLine(key + " " + parsedQuery.Get(key));
                }
            }

            return queryParameters;
        }
    }
}
