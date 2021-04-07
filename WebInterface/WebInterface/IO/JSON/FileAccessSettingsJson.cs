using System.Collections.Generic;

namespace WebInterface.IO.JSON
{
    public class FileAccessess
    {
        public List<string> fileNames { get; set; }
        public string accessType { get; set; }
    }

    public class FileAccessSettingsJSON
    {
        public List<FileAccessess> fileAccessess { get; set; }
    }
}
