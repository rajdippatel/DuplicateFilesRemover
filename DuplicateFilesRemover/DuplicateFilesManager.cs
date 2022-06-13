using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFilesRemover
{
    internal class DuplicateFilesManager
    {
        private Dictionary<string, FileDetail> files = new Dictionary<string, FileDetail>();

        private List<string> scanDirectories = new List<string>();

        public void AddScanDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException(nameof(directory));

            scanDirectories.Add(directory);
        }

        public void Scan()
        {
            ScanFiles();
        }

        private void ScanFiles()
        {
            foreach (var directory in scanDirectories)
            {
                ScanDirectory(new DirectoryInfo(directory));
            }
        }

        private void ScanDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null || !directoryInfo.Exists)
                return;

            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                files.Add(fileInfo.FullName, new FileDetail(fileInfo.FullName));
            }

            foreach (var childDirectoryInfo in directoryInfo.EnumerateDirectories())
            {
                ScanDirectory(childDirectoryInfo);
            }
        }
    }
}
