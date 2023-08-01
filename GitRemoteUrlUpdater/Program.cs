using System;
using System.Diagnostics;
using System.IO;

namespace GitRemoteUrlUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var directoryPath = @"C:\Users\user\source\repos\ParentDirectory";
            var parentDirectoryName = "ParentDirectory";
            var gitRemoteUrl = "git@github.com:username/{0}.git";

            foreach (var directory in Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories))
            {
                var directoryName = new DirectoryInfo(directory).Name;
                if (directoryName == parentDirectoryName)
                {
                    continue;
                }

                var processInfo = new ProcessStartInfo("cmd.exe", $"/C cd {directory} && git remote set-url origin " + string.Format(gitRemoteUrl, directoryName))
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                var process = new Process { StartInfo = processInfo };
                process.Start();
                process.WaitForExit();

                Console.WriteLine($"Git remote url changed for {directoryName}");
            }
        }
    }
}
