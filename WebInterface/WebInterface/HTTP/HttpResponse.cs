using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebInterface.HTTP
{
    public class HttpResponse : HttpBase
    {

        int responseCode;

        public string contentType;

        byte[] data = Array.Empty<byte>();

        string dataPath = "";

        //---------------------Functions---------------------------

        public void AddString(string msg)
        {
            data = Encoding.UTF8.GetBytes(msg);
        }

        public void AddFile(string url)
        {
            if (File.Exists(url))
            {
                dataPath = url;
            }
        }

        public void Redirect(string path)
        {
            responseCode = 301;

            headers.Add("Location", path);
        }

        public void RedirectWithQueryParameters(string path)
        {
            responseCode = 301;

            headers.Add("Location", path);
        }

        public void RedirectWithAuthorization(string path, string token)
        {
            responseCode = 301;

            queryParameters.Add("user", token);

            headers.Add("Location", path + QueryToString());
        }

        //---------------------Encoding---------------------------

        public void WriteToStream(Stream stream)
        {
            try
            {
                string httpHeaders;

                if (dataPath.Length > 0)
                {
                    FileStream fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read);

                    long streamSize = fs.Length;

                    httpHeaders = GenerateHttpHeaders(streamSize);

                    WriteHttpHeadersToStream(httpHeaders, stream);

                    fs.CopyTo(stream);

                    return;
                }

                int dataSize = data.Length;

                httpHeaders = GenerateHttpHeaders(dataSize);

                WriteHttpHeadersToStream(httpHeaders, stream);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception While Writing Response:" + e.ToString());
            }
        }

        string GenerateHttpHeaders(long dataSize)
        {
            //Gets Response Code Discription
            string responseCodeText = HttpResponseCodes.ResponseCodeText(responseCode);

            //Creates And Adds Status Line
            string httpStringData = $"HTTP/1.1 {responseCode} {responseCodeText}\n";

            //If Present Adds Headers
            httpStringData += HeadersToString();

            httpStringData += $"Date: \nServer: {server}\n";

            httpStringData += $"Content-Length: {dataSize}\nContent-Type: {contentType}\n";

            //Adds Data Seperator
            httpStringData += "\n";

            return httpStringData;
        }

        //---------------------Decoding---------------------------

        public static HttpResponse FromStream(Stream stream)
        {
            try
            {
                StreamReader reader = new StreamReader(stream);

                //Gets All Path And Method Info From Request Line
                DecodeStatusLine(reader, out int responseCode);

                //Gets All Pressent Headers
                var headers = GetHeaders(reader);

                return new HttpResponse("test", responseCode, "text/html", headers);

            }
            catch (Exception e)
            {
                Console.WriteLine("Http Decoding Error:" + e.ToString());
                return null;
            }
        }

        static void DecodeStatusLine(StreamReader reader, out int responseCode)
        {
            string requestLine = reader.ReadLine();

            string[] requestLineTokens = requestLine.Split(' ');

            responseCode = int.Parse(requestLineTokens[1]);
        }

        //---------------------Headers----------------------------

        //---------------------Constructors-----------------------

        public HttpResponse(string server, int responseCode, string contentType, Dictionary<string, string> headers)
        {
            this.server = server;
            this.responseCode = responseCode;
            this.contentType = contentType;
            this.headers = headers;
        }

        public HttpResponse(string server, int responseCode, string contentType)
        {
            this.server = server;
            this.responseCode = responseCode;
            this.contentType = contentType;
            headers = new Dictionary<string, string>();
        }

        public HttpResponse(string server)
        {
            this.server = server;
            responseCode = 200;
        }

        public HttpResponse(int responseCode, Dictionary<string, string> headers)
        {
            this.responseCode = responseCode;
            this.headers = headers;
        }


    }
}
