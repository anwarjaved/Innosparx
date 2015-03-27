using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AutoUpdater
{
    public class UpdateInfoEventArgs : EventArgs
    {
        public bool IsUpdateAvailable { get; set; }

        public string DownloadUrl { get; set; }

        public string ChangelogUrl { get; set; }

        public Version CurrentVersion { get; set; }

        public Version InstalledVersion { get; set; }
    }
}
