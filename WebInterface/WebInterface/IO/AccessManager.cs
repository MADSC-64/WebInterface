using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using WebInterface.Authorization;
using WebInterface.IO.JSON;

namespace WebInterface.IO
{
    public static class AccessManager
    {
        static List<FileAccessSettingsJSON> fileAccessSettings = new List<FileAccessSettingsJSON>();
        static List<FolderAccessSettingsJson> folderAccessSettings = new List<FolderAccessSettingsJson>();

        static string programPath;

        public static bool IsFileRestricted(string path,out FileAccessess authorizationFile)
        {
            authorizationFile = GetFileAuthorization(path);

            return (authorizationFile != null);
        }

        public static bool IsAuthorized(string token, FileAccessess authorizationFile)
        {
            if (authorizationFile == null)
                throw new Exception("Trying To Test Authorization To A Unrestricted File");

            Console.WriteLine("Testing Token:" + token);

            return TokenManager.TestToken(token, authorizationFile.accessType);
        }

        static FileAccessess GetFileAuthorization(string path)
        {
            path = path.Replace('/', '\\');

            if (fileAccessSettings.Count > 0)
            {
                foreach (var accessSettingContainer in fileAccessSettings){
                    foreach (var file in accessSettingContainer.fileAccessess){
                        foreach (var filePath in file.fileNames)
                        {
                            string authorizedFilePath = Path.Combine(programPath, filePath);

                            authorizedFilePath = authorizedFilePath.Replace('/','\\');

                            Console.WriteLine(authorizedFilePath + " = " + path);
                            if (path == authorizedFilePath)
                            {
                                Console.WriteLine("Found Match "+ file.accessType);
                                return file;
                            }
                        }
                    }
                }
            }


            return null;
        }

        public static void LoadFileAccessSettings()
        {
            programPath = Environment.CurrentDirectory;

            programPath = Path.Combine(programPath + "/data/Web");

            //Gets All Files With Fileset Prefix
            string[] fileAcessPaths = Directory.GetFiles(programPath, "*.fileset", SearchOption.AllDirectories);

            Console.WriteLine("Loading File Access Settings");

            //Loops Through All Found Files
            foreach (var path in fileAcessPaths)
            {
                //Reads Content From File
                var content = File.ReadAllText(path);

                //Tries To Convert Json To File Access Settings
                var accessSettings = JsonConvert.DeserializeObject<FileAccessSettingsJSON>(content);

                //If Conversion Successful (result not equal to null) Adds Access Setting List
                if (accessSettings != null) fileAccessSettings.Add(accessSettings);
            }

            //Gets All Files With Folderset Prefix
            string[] folderAcessPaths = Directory.GetFiles(programPath, "*.folderset", SearchOption.AllDirectories);

            Console.WriteLine("Loading Folder Access Settings");

            foreach (var path in folderAcessPaths)
            {
                var content = File.ReadAllText(path);

                var accessSettings = JsonConvert.DeserializeObject<FolderAccessSettingsJson>(content);

                folderAccessSettings.Add(accessSettings);
            }
        }
    }

}
