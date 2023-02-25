using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;

namespace MediaOrganizer
{
    internal class FileOrganizer
    {
        private static int imageFiles = 0;
        private static int videoFiles = 0;
        public static async Task OrganizeMediaFiles(string sourceDirectory, Action<int, string> progressCallback, CancellationToken cancellationToken)
        {
            if (!System.IO.Directory.Exists(sourceDirectory))
            {
                progressCallback(0, "Folder does not exist.");
                return;
            }

            List<string> files = System.IO.Directory.EnumerateFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
                .ToList();

            int totalFiles = files.Count;
            int processedFiles = 0;
            double filesPerPercent = totalFiles / 100.0;

            foreach (string file in files)
            {
                ProcessMediaFile(file, sourceDirectory, progressCallback, ref processedFiles, filesPerPercent, cancellationToken);
            }

            string logMessage = $"{Environment.NewLine}{Environment.NewLine}-------------------------------------------{Environment.NewLine}" +
                $"Total Processed Files: {processedFiles}{Environment.NewLine}Image Files: {imageFiles}{Environment.NewLine}Video Files: {videoFiles}";
            imageFiles = 0;
            videoFiles = 0;
            progressCallback(100, logMessage);
        }

        private static void ProcessMediaFile(string file, string sourceDirectory, Action<int, string> progressCallback, ref int processedFiles, double filesPerPercent, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // Throw an OperationCanceledException
                throw new OperationCanceledException(cancellationToken);
            }

            string extension = Path.GetExtension(file);
            if (!IsSupportedFileType(extension))
            {
                return;
            }

            if (IsImage(extension))
            {
                imageFiles++;
            }

            if (IsVideo(extension))
            {
                videoFiles++;
            }

            DateTime mediaDate = GetMediaDateFromExifOrFallback(file);

            string destinationDirectory = Path.Combine(sourceDirectory, mediaDate.ToString("yyyy"), mediaDate.ToString("MMMM"));
            System.IO.Directory.CreateDirectory(destinationDirectory);

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

        public static DateTime GetMediaDateFromExifOrFallback(string filePath)
        {
            try
            {
                var directories = ImageMetadataReader.ReadMetadata(filePath);

                // Try to get media creation date from QuickTime/MP4 metadata
                var quickTimeDirectory = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
                if (quickTimeDirectory != null && quickTimeDirectory.TryGetDateTime(QuickTimeMovieHeaderDirectory.TagCreated, out DateTime mediaDate))
                {
                    return mediaDate;
                }

                // Try to get media creation date from Exif metadata
                var exifSubIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                if (exifSubIfdDirectory != null && exifSubIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime exifDate))
                {
                    return exifDate;
                }

                // Fallback to file creation date
                return File.GetCreationTime(filePath);
            }
            catch (Exception)
            {
                // Error occurred while reading metadata
                return File.GetCreationTime(filePath);
            }
        }
    }
}
