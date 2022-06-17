using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFilesRemover
{
    internal class DuplicateFileGroup
    {
        public File[] Files { get; private set; }

        public DuplicateFileGroup()
        {
            Files = new File[0];
        }

        public bool AddFile(File file)
        {
            if (Files.Contains(file))
                return false;

            file.DuplicateFileGroup = this;

            var newFiles = new List<File>(Files);
            newFiles.Add(file);
            Files = newFiles.ToArray();

            Console.WriteLine("File Added as duplicated :- Size : " + file.Size +  ", Path : " + file.Path);

            return true;
        }
    }
}
