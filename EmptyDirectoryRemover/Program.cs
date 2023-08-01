namespace EmptyDirectoryRemover
{
    internal class Program
    {
        static void Main()
        {
            string rootDirectory;
            rootDirectory = @"V:\Family-Full\Images\Uncategorized";
            rootDirectory = @"V:\Special";
            rootDirectory = @"V:\Raj";

            // Check if the root directory exists
            if (Directory.Exists(rootDirectory))
            {
                DeleteEmptyDirectories(rootDirectory);
                Console.WriteLine("Empty directories have been deleted.");
            }
            else
            {
                Console.WriteLine("The root directory does not exist.");
            }
        }

        static bool DeleteEmptyDirectories(string directory)
        {
            try
            {
                start:
                // Get all subdirectories in the current directory
                string[] subdirectories = Directory.GetDirectories(directory);

                foreach (string subdirectory in subdirectories)
                {
                    // Recursively call the function for each subdirectory
                    if (DeleteEmptyDirectories(subdirectory))
                        goto start;
                }

                // Get all files in the current directory
                string[] files = Directory.GetFiles(directory);

                // If there are no files and subdirectories, the directory is empty, and we can delete it
                if (subdirectories.Length == 0 && files.Length == 0)
                {
                    Directory.Delete(directory);
                    Console.WriteLine("Deleted empty directory: " + directory);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that may occur during the directory deletion process
                Console.WriteLine("Error while deleting directory: " + ex.Message);
            }
            return false;
        }
    }
}