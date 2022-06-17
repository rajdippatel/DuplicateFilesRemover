using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFilesRemover
{
    internal class File
    {
        public string Path { get; }

        public long Size { get; set; }

        public DuplicateFileGroup DuplicateFileGroup { get; set; }

        public File(string path)
        {
            Path = path;
        }
    }
}
