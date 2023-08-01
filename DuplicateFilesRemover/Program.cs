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
                // duplicateFilesManager.AddScanDirectory(@"V:\Family-Full\Images\");
                //duplicateFilesManager.AddScanDirectory(@"V:\M.K.Patel\");
                //duplicateFilesManager.AddScanDirectory(@"V:\Dharmi\");
                //duplicateFilesManager.AddScanDirectory(@"V:\A\");
                //duplicateFilesManager.AddScanDirectory(@"V:\Jaydip\");
                duplicateFilesManager.AddScanDirectory(@"V:\Raj\Backup");
                //duplicateFilesManager.AddScanDirectory(@"V:\Jignesha\");
                //duplicateFilesManager.AddScanDirectory(@"V:\Special\");
                //duplicateFilesManager.AddScanDirectory(@"V:\Entertainment\");
                //duplicateFilesManager.AddScanDirectory(@"V:\Family-Full\Images\Uncategorized\");
                //duplicateFilesManager.AddScanDirectory(@"V:\Raj\BOOKS");
                //duplicateFilesManager.AddScanDirectory(@"C:\Users\RAJ\Desktop\Test");
                //duplicateFilesManager.AddScanDirectory(@"C:\Users\RAJ\Desktop\Test");
                //duplicateFilesManager.AddScanDirectory(@"V:\Family-Full\Images\Dipika");
                //duplicateFilesManager.AddScanDirectory(@"V:\Raj\OneDrive");
                //duplicateFilesManager.AddScanDirectory(@"V:\Raj\OneDrive - Backup");
                //duplicateFilesManager.AddScanDirectory(@"V:\Raj\OneDrive - Copy");
                //duplicateFilesManager.AddScanDirectory(@"X:\");
                //duplicateFilesManager.AddScanDirectory(@"D:\Raj\Backup");
                duplicateFilesManager.ScanWithDelete();
                // duplicateFilesManager.RemoveDuplicates();
            }

            //{
            //    var duplicateFilesManager = new DuplicateFilesManager();
            //    duplicateFilesManager.AddScanDirectory(@"Y:\");
            //    duplicateFilesManager.ScanWithDelete();
            //}

            //{
            //    var duplicateFilesManager = new DuplicateFilesManager();
            //    
            //    duplicateFilesManager.ScanWithDelete();
            //}

        }
    }
}
