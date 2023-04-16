using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FixWindowsCopiedFileNames
{
    internal class FixWindowsCopiedFileNamesManager
    {
        private List<string> scanDirectories = new List<string>();
        private Dictionary<string, string> files = new Dictionary<string, string>();
        private string regEx = "(.*?) [(][0-9]+[)]";

        public void AddScanDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException(nameof(directory));
            
            scanDirectories.Add(directory);
        }

        private void LoadFiles()
        {
            foreach (var directory in scanDirectories)
            {
                LoadDirectory(new DirectoryInfo(directory));
            }
        }

        private void LoadDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null || !directoryInfo.Exists)
                return;

            try
            {
                foreach (var fileInfo in directoryInfo.EnumerateFiles())
                {
                    files.Add(fileInfo.FullName, fileInfo.FullName);
                }

                foreach (var childDirectoryInfo in directoryInfo.EnumerateDirectories())
                {
                    LoadDirectory(childDirectoryInfo);
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void ScanWithRename()
        {
            LoadFiles();
            foreach (var file in files)
            {
                ProcessFile(file.Value);
            }
        }

        private void ProcessFile(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            
            var match = Regex.Match(fileName, regEx);
            if (match.Success)
            {
                if(match.Groups.Count > 0)
                {
                    var fileNameWithoutIndex = match.Groups[1];

                    var newFilePath = Path.Combine(Path.GetDirectoryName(filePath), fileNameWithoutIndex.Value + Path.GetExtension(filePath));
                    if (!File.Exists(newFilePath))
                    {
                        File.Move(filePath, newFilePath);
                        Console.WriteLine("File Renamed :- " + filePath + " --> " + newFilePath);
                        ProcessFile(newFilePath);
                    }
                }
            }
        }
    }
}
