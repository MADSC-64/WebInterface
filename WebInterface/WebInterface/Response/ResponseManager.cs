using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebInterface.API;
using WebInterface.HTTP;
using WebInterface.IO;

namespace WebInterface.Response
{
    public static class ResponseManager
    {

        static Dictionary<string, Dictionary<(string, int), IRequestInterface>> httpResponseInterfaces = new Dictionary<string, Dictionary<(string, int), IRequestInterface>>();


        public static HttpResponse GenerateResponse(HttpRequest request, Server server)
        {
            if (request == null) return HttpErrors.GenerateHttpError(400);

            string path = request.path.LocalPath;
            string method = request.method;
            int port = server.port;


            string parentFolder = request.path.Parent().LocalPath;


            //Tries To Find Interface
            IRequestInterface requestInterface = FindInterface(port, path, method, parentFolder);

            //If Found Calls Interface Method And Returns Response
            if (requestInterface != null) return requestInterface.OnRecieveRequest(request);

            return CreateDefaultResponse(request);
        }

        public static HttpResponse CreateDefaultResponse(HttpRequest request)
        {
            if (request.method == "GET") return FileManager.ReadFile(request);

            if (request.method == "SET") return FileManager.WriteFile(request);

            return HttpErrors.GenerateHttpError(400);
        }

        /// <summary>
        /// Finds Interface From Dictionary 
        /// </summary>
        public static IRequestInterface FindInterface(int port, string path, string method, string parentFolder)
        {
            if (httpResponseInterfaces.ContainsKey(path))
            {
                var interfaceContainer = httpResponseInterfaces[path];

                if (interfaceContainer.ContainsKey((method, port)))
                    return interfaceContainer[(method, port)];

                if (interfaceContainer.ContainsKey((method, 0)))
                    return interfaceContainer[(method, 0)];

                if (interfaceContainer.ContainsKey(("", port)))
                    return interfaceContainer[("", port)];

                if (interfaceContainer.ContainsKey(("", 0)))
                    return interfaceContainer[("", 0)];
            }

            if (httpResponseInterfaces.ContainsKey(parentFolder + '*'))
            {
                var interfaceContainer = httpResponseInterfaces[parentFolder + '*'];

                if (interfaceContainer.ContainsKey((method, port)))
                    return interfaceContainer[(method, port)];

                if (interfaceContainer.ContainsKey((method, 0)))
                    return interfaceContainer[(method, 0)];

                if (interfaceContainer.ContainsKey(("", port)))
                    return interfaceContainer[("", port)];

                if (interfaceContainer.ContainsKey(("", 0)))
                    return interfaceContainer[("", 0)];
            }

            //If Not Found Retuns Null
            return null;
        }

        public static void InitiateResponseInterfaces()
        {
            //Gets All Request Interface Instances
            var interfaces = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(a => a.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .OfType<IRequestInterface>();

            int loadedInterfaces = 0;

            //Add All Instances To Dictionary
            foreach (var instance in interfaces)
            {
                int port = instance.Port;
                string path = instance.Path;
                string method = instance.Method;


                if (!httpResponseInterfaces.ContainsKey(path))
                    httpResponseInterfaces.Add(path, new Dictionary<(string, int), IRequestInterface>());

                var interfaceContainer = httpResponseInterfaces[path];

                interfaceContainer.Add((method, port), instance);


                loadedInterfaces++;
            }

            Console.WriteLine(": Loaded " + loadedInterfaces + " Interfaces");

        }
    }
}
