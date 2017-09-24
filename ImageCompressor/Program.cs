namespace ImageCompressor
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using ImageProcessor;

    class Program
    {
        static void CompressImageHelper(string originalImgFile, string outImageFile)
        {
            // Read a originalImgFile and resize it.
            byte[] photoBytes = File.ReadAllBytes(originalImgFile);
            int quality = 25;

            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory())
                    {
                        // Load, resize, set the format and quality and save an image.
                        var imgFactory = imageFactory.Load(inStream);
                        var size = new Size(imgFactory.Image.Width / 2, imgFactory.Image.Height / 2);

                        // .Resize(size)
                        imgFactory
                            //.Resize(size)
                            .Pixelate(5)
                            //.Quality(quality)
                            .Save(outStream);
                    }


                    using (FileStream file = new FileStream(outImageFile, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        outStream.WriteTo(file);
                    }
                }
            }
        }

        static void CompressImages(string targetFolder)
        {
            const string CompressFolderName = "compressed";
            var outputFolder = Path.Combine(targetFolder, CompressFolderName);
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            var originalImages = Directory.GetFiles(targetFolder).Where(x => x.EndsWith(".png"));
            foreach (var originalImage in originalImages)
            {
                var fileName = originalImage.Split('\\').Last();
                CompressImageHelper(originalImage, Path.Combine(outputFolder, fileName));
                Console.WriteLine("Finished " + fileName);
            }
        }

        static void Main(string[] args)
        {
            var projectFolder = @"C:\Users\Rob\Source\Repos\EmilyChenProfileWeb\src\assets\projects";
            var projectFolders = Directory.GetDirectories(projectFolder);
            foreach (var folder in projectFolders)
            {
                CompressImages(folder);
            }
        }
    }
}