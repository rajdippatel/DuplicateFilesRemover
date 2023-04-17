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
            {
                var duplicateFilesManager = new DuplicateFilesManager();
                duplicateFilesManager.AddScanDirectory(@"V:\Family-Full\Images\");
                duplicateFilesManager.AddScanDirectory(@"V:\M.K.Patel\");
                duplicateFilesManager.AddScanDirectory(@"V:\Dharmi\");
                duplicateFilesManager.AddScanDirectory(@"V:\Jaydip\");
                duplicateFilesManager.AddScanDirectory(@"V:\Jignesha\");
                duplicateFilesManager.AddScanDirectory(@"V:\Special\");
                duplicateFilesManager.AddScanDirectory(@"V:\Entertainment\");
                duplicateFilesManager.ScanWithDelete();
                // duplicateFilesManager.RemoveDuplicates();
            }

            //{
            //    var duplicateFilesManager = new DuplicateFilesManager();
            //    
            //    duplicateFilesManager.ScanWithDelete();
            //}

        }
    }
}
