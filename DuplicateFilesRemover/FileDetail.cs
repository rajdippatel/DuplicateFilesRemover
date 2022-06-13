using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFilesRemover
{
    internal class FileDetail
    {
        public string Path { get; }

        public long Size { get; set; }

        public FileDetail(string path)
        {
            Path = path;
        }
    }
}
