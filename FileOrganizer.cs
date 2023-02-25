using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;
using System.Text;

namespace MediaOrganizer
{
    internal class FileOrganizer
    {
        public static void OrganizeMediaFiles(string sourceDirectory, Action<int, string> progressCallback)
        {
            if (!Directory.Exists(sourceDirectory))
            {
                progressCallback(0, "Folder does not exist.");
                return;
            }

            List<string> files = Directory.EnumerateFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
                .ToList();

            int totalFiles = files.Count;
            int processedFiles = 0;
            double filesPerPercent = totalFiles / 100.0;

            foreach (string file in files)
            {
                ProcessMediaFile(file, sourceDirectory, progressCallback, ref processedFiles, filesPerPercent);
            }
        }

        private static void ProcessMediaFile(string file, string sourceDirectory, Action<int, string> progressCallback, ref int processedFiles, double filesPerPercent)
        {
            string extension = Path.GetExtension(file);
            if (!IsSupportedFileType(extension))
            {
                return;
            }

            DateTime mediaDate = GetMediaDateFromExifOrFallback(file, extension);

            string destinationDirectory = Path.Combine(sourceDirectory, mediaDate.ToString("yyyy"), mediaDate.ToString("MMMM"));
            Directory.CreateDirectory(destinationDirectory);

            string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
            if (File.Exists(destinationFile))
            {
                int suffix = 1;
                string baseName = Path.GetFileNameWithoutExtension(file);
                string newFileName = "";

                while (File.Exists(destinationFile))
                {
                    newFileName = $"{baseName}_({suffix}){extension}";
                    destinationFile = Path.Combine(destinationDirectory, newFileName);
                    suffix++;
                }
            }

            File.Move(file, destinationFile);

            // Update progress and logs
            processedFiles++;
            int progressPercentage = (int)Math.Round(processedFiles / filesPerPercent);
            string logMessage = $"Processed file: {file}";
            progressCallback(progressPercentage, logMessage);
        }

        static bool IsSupportedFileType(string extension)
        {
            return IsImage(extension) || IsVideo(extension);
        }

        static bool IsImage(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                case ".tiff":
                    return true;
                default:
                    return false;
            }
        }

        static bool IsVideo(string extension)
        {
            switch (extension.ToLower())
            {
                case ".mp4":
                case ".avi":
                case ".wmv":
                case ".mov":
                    return true;
                default:
                    return false;
            }
        }

        static DateTime GetMediaDateFromExifOrFallback(string file, string extension)
        {
            try
            {
                // Try to get the taken date from the EXIF metadata
                using (var image = new Bitmap(file))
                {
                    var propertyItem = image.GetPropertyItem(0x9003); // DateTimeOriginal tag
                    var takenDate = new DateTime(
                        int.Parse(Encoding.ASCII.GetString(propertyItem.Value, 0, 4)),
                        int.Parse(Encoding.ASCII.GetString(propertyItem.Value, 5, 2)),
                        int.Parse(Encoding.ASCII.GetString(propertyItem.Value, 8, 2)),
                        int.Parse(Encoding.ASCII.GetString(propertyItem.Value, 11, 2)),
                        int.Parse(Encoding.ASCII.GetString(propertyItem.Value, 14, 2)),
                        int.Parse(Encoding.ASCII.GetString(propertyItem.Value, 17, 2)));
                    return takenDate;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and fallback to the creation time
                Console.WriteLine($"Error getting taken date from EXIF metadata for {file}: {ex.Message}");
                var fileShellObject = ShellObject.FromParsingName(file);
                var properties = fileShellObject?.Properties?.DefaultPropertyCollection?.Select(property => property);
                var creationTimeProperty = properties?.FirstOrDefault(property => property.CanonicalName == "System.DateCreated");
                if (creationTimeProperty != null && creationTimeProperty.ValueAsObject != null)
                {
                    return (DateTime)creationTimeProperty.ValueAsObject;
                }
                return File.GetCreationTime(file);
            }
        }
    }
}
