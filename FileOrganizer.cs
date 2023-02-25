using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;

namespace MediaOrganizer
{
    internal class FileOrganizer
    {
        static void OrganizeMediaFiles(string sourceDirectory, Action<int, string> progressCallback)
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
                processedFiles++;

                string extension = Path.GetExtension(file);
                if (!IsSupportedFileType(extension))
                {
                    continue;
                }

                string mediaDate = GetMediaDateFromExif(file, extension) ?? File.GetCreationTime(file).ToString("yyyy-MM-dd");

                string destinationDirectory = Path.Combine(sourceDirectory, mediaDate.Substring(0, 4), mediaDate.Substring(5, 2));
                Directory.CreateDirectory(destinationDirectory);

                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                if (File.Exists(destinationFile))
                {
                    Console.WriteLine($"File {Path.GetFileName(file)} already exists in {destinationDirectory}.");
                }
                else
                {
                    File.Move(file, destinationFile);
                }

                // Update progress and logs
                processedFiles++;
                int progressPercentage = (int)Math.Round(processedFiles / filesPerPercent);
                string logMessage = $"Processed file: {file}";
                progressCallback(progressPercentage, logMessage);
            }
        }

        static bool IsSupportedFileType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                case ".tiff":
                case ".mp4":
                case ".avi":
                case ".wmv":
                case ".mov":
                    return true;
                default:
                    return false;
            }
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

        static string? GetMediaDateFromExif(string file, string extension)
        {
            if (IsVideo(extension))
            {
                using var shell = ShellObject.FromParsingName(file);
                var mediaProperties = shell?.Properties?.DefaultPropertyCollection?.Select(property => property);

                var dateTaken = mediaProperties?.FirstOrDefault(p => p.CanonicalName == "System.Media.DateEncoded")?.ValueAsObject as DateTime?;
                if (dateTaken != null)
                {
                    return dateTaken.Value.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                using var image = new Bitmap(file);
                var propertyItem = image.GetPropertyItem(36867);
                if (propertyItem != null && propertyItem.Value != null)
                {
                    string dateTaken = System.Text.Encoding.ASCII.GetString(propertyItem.Value);
                    return dateTaken[..10];
                }
            }

            return null;
        }
    }
}
