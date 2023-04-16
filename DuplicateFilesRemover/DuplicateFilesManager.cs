using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFilesRemover
{
    internal class DuplicateFilesManager
    {
        private Dictionary<string, File> files = new Dictionary<string, File>();
        private List<DuplicateFileGroup> duplicateFileGroups = new List<DuplicateFileGroup>();

        private List<string> scanDirectories = new List<string>();

        private const long bufferSize = 1 * 1024;

        private byte[] buffer1 = new byte[bufferSize];
        private byte[] buffer2 = new byte[bufferSize];

        private bool SkipZeroLengthFiles = true;
        public bool DeleteInstantly { get; set; } = false;

        public void AddScanDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException(nameof(directory));

            scanDirectories.Add(directory);
        }

        public void Scan()
        {
            LoadFiles();
            ScanDuplicates();
        }

        public void ScanWithDelete()
        {
            DeleteInstantly = true;
            Scan();
        }

        public void RemoveDuplicates()
        {
            foreach (var duplicateFileGroup in duplicateFileGroups)
            {
                var sortedFilePaths = duplicateFileGroup.Files.Select(o => o.Path).ToList();
                sortedFilePaths.Sort();

                for (int i = 1; i < sortedFilePaths.Count; i++)
                {
                    var fileInfo = new FileInfo(sortedFilePaths[i]);
                    if (fileInfo.Exists)
                    {
                        Console.WriteLine("File Deleted :- Size : " + fileInfo.Length + ", Path : " + sortedFilePaths[i]);
                        fileInfo.Delete();
                    }
                }
            }
        }

        private static int FilePathSorter(string filePath1, string filePath2)
        {
            int result;
            var filePath1Parts = Path.GetDirectoryName(filePath1).Split(Path.DirectorySeparatorChar);
            var filePath2Parts = Path.GetDirectoryName(filePath2).Split(Path.DirectorySeparatorChar);

            for (int i = 0; i < Math.Max(filePath1Parts.Length, filePath1Parts.Length); i++)
            {
                string filePath1Part = i < filePath1Parts.Length ? filePath1Parts[i] : string.Empty;
                string filePath2Part = i < filePath2Parts.Length ? filePath2Parts[i] : string.Empty;
                
                result = filePath1Part.CompareTo(filePath2Part);
                if (result != 0)
                {
                    return result;
                }
            }

            var fileName1 = Path.GetFileNameWithoutExtension(filePath1);
            var fileName2 = Path.GetFileNameWithoutExtension(filePath2);

            result = fileName1.CompareTo(fileName2);
            if (result != 0)
            {
                return result;
            }

            var fileExtension1 = Path.GetFileNameWithoutExtension(filePath1);
            var fileExtension2 = Path.GetFileNameWithoutExtension(filePath2);

            result = fileExtension1.CompareTo(fileExtension2);
            if (result != 0)
            {
                return result;
            }
            return result;
        }

        public void RemoveDuplicates(DuplicateFileGroup duplicateFileGroup)
        {
            var sortedFilePaths = duplicateFileGroup.Files.Select(o => o.Path).ToList();
            sortedFilePaths.Sort(FilePathSorter);

            for (int i = 1; i < sortedFilePaths.Count; i++)
            {
                var fileInfo = new FileInfo(sortedFilePaths[i]);
                if (fileInfo.Exists)
                {
                    long fileLength = fileInfo.Length;
                    fileInfo.Delete();
                    Console.WriteLine("File Deleted :- Size : " + fileLength + ", Path : " + sortedFilePaths[i]);
                }
            }
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
                    var fileDetail = new File(fileInfo.FullName) { Size = fileInfo.Length };
                    files.Add(fileDetail.Path, fileDetail);
                }

                foreach (var childDirectoryInfo in directoryInfo.EnumerateDirectories())
                {
                    LoadDirectory(childDirectoryInfo);
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void ScanDuplicates()
        {
            var filesBySize = files.GroupBy(o => o.Value.Size).Where(o => o.Count() > 1).Select(o => o.Select(o1 => o1.Value).ToList()).ToList();
            // OrderByFileSize
            filesBySize = filesBySize.OrderByDescending(o => o.First().Size).ToList();

            for (int i = 0; i < filesBySize.Count; i++)
            {
                Console.WriteLine("Progress : " + (i * 100/ filesBySize.Count)  + "%");
                List<File>? files = filesBySize[i];
                foreach (var file in files)
                {
                    if (SkipZeroLengthFiles)
                    {
                        if (file.Size == 0)
                            continue;
                    }

                    foreach (var otherFile in files)
                    {
                        if (file == otherFile)
                            continue;

                        if(!(new FileInfo(file.Path).Exists && new FileInfo(otherFile.Path).Exists))
                            continue;

                        var sameFiles = AreFilesSame(file, otherFile);
                        if(sameFiles)
                        {
                            var duplicateFileGroup = file.DuplicateFileGroup;
                            if (duplicateFileGroup == null)
                                duplicateFileGroup = otherFile.DuplicateFileGroup;
                            if (duplicateFileGroup == null)
                            {
                                duplicateFileGroup = new DuplicateFileGroup();
                                duplicateFileGroups.Add(duplicateFileGroup);
                            } 
                            duplicateFileGroup.AddFile(file);
                            duplicateFileGroup.AddFile(otherFile);

                            if (DeleteInstantly)
                            {
                                RemoveDuplicates(duplicateFileGroup);
                            }
                        }
                    }
                }
            }
        }

        private bool AreFilesSame(File file, File otherFile)
        {
            using (var fileStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read))
            using (var otherFileStream = new FileStream(otherFile.Path, FileMode.Open, FileAccess.Read))
            {
                long totalBytesMatched = 0;
                while (true)
                {
                    var readCount1 = ReadBufferFully(fileStream, buffer1);
                    var readCount2 = ReadBufferFully(otherFileStream, buffer2);

                    if (readCount1 != readCount2)
                        return false;

                    if (readCount1 == 0)
                        return totalBytesMatched == file.Size;

                    for (int i = 0; i < readCount1; i++)
                    {
                        if (buffer1[i] != buffer2[i])
                        {
                            return false;
                        }
                        else
                        {
                            totalBytesMatched++;
                        }
                    }
                }
            }
        }

        private int ReadBufferFully(FileStream fileStream, byte[] buffer)
        {
            var offset = 0;
            var bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, offset, buffer.Length - offset)) > 0)
            {
                offset += bytesRead;
            }
            return offset;
        }
    }
}
