using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FixWindowsCopiedFileNames
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var fixWindowsCopiedFileNamesManager = new FixWindowsCopiedFileNamesManager();
            fixWindowsCopiedFileNamesManager.AddScanDirectory(@"V:\Family-Full\Images\Uncategorized");
            fixWindowsCopiedFileNamesManager.ScanWithRename();
        }
    }
}
