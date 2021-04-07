using System.Collections.Generic;

namespace WebInterface.IO.JSON
{
    class FolderAccessSettingsJson
    {
        public List<FolderAccessSetting> folderAccesses = new List<FolderAccessSetting>();
    }

    public class FolderAccessSetting
    {
        public string Path;
        public string AccessType;
        public bool includeChildren;
    }
}
