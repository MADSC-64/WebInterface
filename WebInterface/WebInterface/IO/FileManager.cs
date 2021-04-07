using System;
using System.IO;
using WebInterface.HTTP;

namespace WebInterface.IO
{
    public static class FileManager
    {
        static string programPath = "";

        public static HttpResponse ReadFile(HttpRequest request)
        {
            if (programPath == "")
            {
                programPath = Environment.CurrentDirectory;
                programPath = Path.Combine(programPath + "/data/Web");
            }

            //Gets Local Request File
            string path = request.path.LocalPath;

            //Removes The First Symbol for Better Path Combining
            string trimmedPath = path.Remove(0, 1);

            //Combines Program And Request Paths Into One
            string filePath = Path.Combine(programPath, trimmedPath);

            //If File Is Not Found Returns 404
            if (!File.Exists(filePath)) return HttpErrors.GenerateHttpError(404);

            //If File Access Is Restricted And Not Authorized Returns 401
            if (AccessManager.IsFileRestricted(filePath,out JSON.FileAccessess authorizationFile))
                if (!IsAuthorized(request, filePath, authorizationFile)) return HttpErrors.GenerateHttpError(401);

            //Creates New Http Response
            HttpResponse response = new HttpResponse("test", 200, "text/html");

            //Adds File As Http Response Body
            response.AddFile(filePath);

            return response;
        }

        static bool IsAuthorized(HttpRequest request,string filePath, JSON.FileAccessess authorizationFile)
        {
            //If Request Contains Authorization Header Reads Data From It And Tests Token
            if (request.headers.ContainsKey("Authorization"))
                return AccessManager.IsAuthorized(request.headers["Authorization"], authorizationFile);

            //If Request Contains Authorization Query Reads Data From It And Tests Token
            if (request.queryParameters.ContainsKey("user"))
                return AccessManager.IsAuthorized(request.queryParameters["user"], authorizationFile);

            return false;
        }

        public static HttpResponse WriteFile(HttpRequest request)
        {
            //Gets The Folder Of Executing Program
            if (programPath == "") programPath = Environment.CurrentDirectory;

            //Prepares Path For Usage By Removing First Symbol
            string trimmedPath = request.path.LocalPath;

            //Gets The Folder Of Executing Program
            string filePath = Path.Combine(programPath, trimmedPath);

            if (!File.Exists(filePath)) return HttpErrors.GenerateHttpError(404);

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(fs);

                writer.Write(request.data);

                fs.Close();
            }

            HttpResponse response = new HttpResponse("test", 200, "text/html");

            return response;
        }



    }
}
