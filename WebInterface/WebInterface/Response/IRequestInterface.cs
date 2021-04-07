namespace WebInterface.Response
{
    public interface IRequestInterface
    {
        //Specify At what path should it be called
        string Path { get; set; }

        //Specify at what method it be called leave blank if any
        string Method { get; set; }

        //Specify at what method it be called set to 0 if any
        int Port { get; set; }


        /// <summary>
        /// Gets Called When A Request Matching Path And Method Are Found
        /// </summary>
        /// <param name="request"> Contains The Http Request </param>
        /// <returns> Http Response </returns>
        public abstract HTTP.HttpResponse OnRecieveRequest(HTTP.HttpRequest request);

    }


}
