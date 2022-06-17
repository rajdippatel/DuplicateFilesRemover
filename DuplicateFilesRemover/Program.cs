using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFilesRemover
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var duplicateFilesManager = new DuplicateFilesManager();
            duplicateFilesManager.AddScanDirectory(@"V:\Family-Full\Images\Uncategorized");
            // duplicateFilesManager.AddScanDirectory(@"W:\");
            // duplicateFilesManager.AddScanDirectory(@"D:\Test-2");
            duplicateFilesManager.Scan();
            // duplicateFilesManager.RemoveDuplicates();
        }
    }
}
