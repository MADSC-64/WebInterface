namespace WebInterface.HTTP
{
    class HttpErrors
    {
        //Creates Http Error Response
        public static HttpResponse GenerateHttpError(int errorCode)
        {
            HttpResponse response = new HttpResponse("server", errorCode, "text/html");

            response.AddString("<!DOCTYPE HTML P>< html >< head >< title > 404 Not Found </ title ></ head >< body >< h1 > Not Found </ h1 >< p > The requested URL requested was not found on this server.</ p ></ body ></ html > ");

            return response;
        }
    }
}
