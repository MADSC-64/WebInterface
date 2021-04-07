using System;
using System.Collections.Generic;
using System.Linq;

namespace WebInterface.HTTP
{
    public static class UrlProcessing
    {
        public static Uri Parent(this Uri uri)
        {
            return new Uri(uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length - uri.Query.Length).TrimEnd('/'));
        }

        public static Uri GetAbsoluteUrl(string path, Dictionary<string, string> headers)
        {
            string host = headers["Host"];

            string absolutePath = "http://" + host + path;

            return new Uri(absolutePath, UriKind.Absolute);
        }
    }
}
