using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Drawing;

namespace IdenticalImageDetector
{
    internal class Program
    {
        static void Main()
        {
            string directoryPath = @"V:\Family-Full\Images\Uncategorized\Jaydip iPhone\2023-07-21";

            // Check if the directory exists
            if (Directory.Exists(directoryPath))
            {
                var identicalImages = FindIdenticalImages(directoryPath);
                if (identicalImages.Length == 0)
                {
                    Console.WriteLine("No identical images found.");
                }
                else
                {
                    Console.WriteLine("Identical images found:");
                    foreach (var imagePair in identicalImages)
                    {
                        Console.WriteLine($"{imagePair.Item1} and {imagePair.Item2}");
                    }
                }
            }
            else
            {
                Console.WriteLine("The directory does not exist.");
            }
        }

        static Tuple<string, string>[] FindIdenticalImages(string directoryPath)
        {
            var identicalImages = new List<Tuple<string, string>>();
            var imageFiles = Directory.GetFiles(directoryPath, "*.jpg", SearchOption.AllDirectories);

            // Dictionary to store the hash value and file path of each image
            var imageHashes = new Dictionary<string, string>();

            foreach (var imageFile in imageFiles)
            {
                try
                {
                    string hash = GetImageHash(imageFile);
                    if (!imageHashes.ContainsKey(hash))
                    {
                        // Add the hash value and file path to the dictionary
                        imageHashes.Add(hash, imageFile);
                    }
                    else
                    {
                        // The image is identical to another image with the same hash value
                        identicalImages.Add(new Tuple<string, string>(imageFile, imageHashes[hash]));
                    }
                }
                catch(Exception ex)
                {

                }
            }

            return identicalImages.ToArray();
        }

        static string GetImageHash(string imagePath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                
                return CalculateMD5Hash(image);
                // Convert the image to a fixed-size bitmap (e.g., 8x8) to generate a consistent hash value
                using (var resizedImage = ResizeImage(image, 8, 8))
                {
                    return CalculateMD5Hash(resizedImage);
                }
            }
        }

        static Image ResizeImage(Image image, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }

        static string CalculateMD5Hash(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Jpeg);
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(memoryStream.ToArray());
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}