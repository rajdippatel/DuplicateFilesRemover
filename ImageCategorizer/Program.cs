using ExifLibrary;
using FaceRecognitionDotNet;
using MetadataExtractor;
using System.Reflection;
using TagLib.Image;
using File = System.IO.File;

namespace ImageCategorizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var path = @"C:\Users\RAJ\Desktop\2.jpg";

            //var tagFile = TagLib.File.Create(path);
            //if(tagFile.Tag is CombinedImageTag imageTag)
            //{
            //    // File.Delete(path);
            //    imageTag.Keywords = new string[] { "MatajiA", "Bhavani" };
            //}
            //tagFile.Save();
            

            var faceRecognition = FaceRecognition.Create("models");
            var inputDirectory = "V:\\Family-Full\\Images\\Uncategorized\\Jaydip iPhone\\2023-07-21\\DCIM1\\202303__";

            foreach (var file in System.IO.Directory.EnumerateFiles(inputDirectory, "*.jpg"))
            {
                var metadataFile = ImageFile.FromFile(file);
                var keywords = metadataFile.Properties.Get<WindowsByteString>(ExifTag.WindowsKeywords);
                if (keywords != null && (keywords.Value.Contains("Human") || keywords.Value.Contains("No-Human")))
                    continue;


                using (var unknownImage = FaceRecognition.LoadImageFile(file))
                {
                    var faceLocations = faceRecognition.FaceLocations(unknownImage, 0, Model.Cnn).ToArray();
                    var faceValue = faceLocations.Length > 0 ? "Human" : "No-Human";
                    var value = keywords != null ? keywords.Value : "";
                    if (value.Length > 0)
                        value += ";";
                    value += faceValue + ";";
                    
                    metadataFile.Properties.Remove(ExifTag.WindowsKeywords);
                    metadataFile.Properties.Set(ExifTag.WindowsKeywords, new WindowsByteString(ExifTag.WindowsKeywords, new WindowsByteString(ExifTag.WindowsKeywords, value)));
                    metadataFile.Save(file);
                }
            }
        }
    }
}