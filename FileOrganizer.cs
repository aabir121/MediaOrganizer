using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;

namespace MediaOrganizer
{
    internal class FileOrganizer
    {
        private static int _imageFiles = 0;
        private static int _videoFiles = 0;
        public static async Task OrganizeMediaFiles(RunConfig runConfig, Action<int, string> progressCallback, CancellationToken cancellationToken)
        {
            if (!System.IO.Directory.Exists(runConfig.SourceDirectory))
            {
                progressCallback(0, "Folder does not exist.");
                return;
            }

            var files = System.IO.Directory.EnumerateFiles(runConfig.SourceDirectory, "*.*", SearchOption.AllDirectories)
                .ToList();

            var totalFiles = files.Count;
            var processedFiles = 0;
            var filesPerPercent = totalFiles / 100.0;

            foreach (var file in files)
            {
                ProcessMediaFile(file, runConfig, progressCallback, ref processedFiles, filesPerPercent, cancellationToken);
            }

            if (runConfig.RemoveEmptyDirectory)
            {
                DeleteEmptyDirectories(runConfig.SourceDirectory);
            }

            var logMessage = $"{Environment.NewLine}{Environment.NewLine}-------------------------------------------{Environment.NewLine}" +
                             $"Total Processed Files: {processedFiles}{Environment.NewLine}Image Files: {_imageFiles}{Environment.NewLine}Video Files: {_videoFiles}";
            _imageFiles = 0;
            _videoFiles = 0;
            progressCallback(100, logMessage);
        }

        private static void DeleteEmptyDirectories(string directory)
        {
            foreach (var subDirectory in System.IO.Directory.GetDirectories(directory))
            {
                DeleteEmptyDirectories(subDirectory);
            }

            if (System.IO.Directory.GetFiles(directory).Length == 0 &&
                System.IO.Directory.GetDirectories(directory).Length == 0)
            {
                System.IO.Directory.Delete(directory, false);
            }
        }

        private static void ProcessMediaFile(string file, RunConfig runConfig, Action<int, string> progressCallback, ref int processedFiles, double filesPerPercent, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // Throw an OperationCanceledException
                throw new OperationCanceledException(cancellationToken);
            }

            var extension = Path.GetExtension(file);
            if (!IsSupportedFileType(extension))
            {
                return;
            }

            if (IsImage(extension))
            {
                _imageFiles++;
            }

            if (IsVideo(extension))
            {
                _videoFiles++;
            }

            var mediaDate = GetMediaDateFromExifOrFallback(file);

            var destinationDirectory = Path.Combine(runConfig.SourceDirectory, mediaDate.ToString("yyyy"), mediaDate.ToString("MMMM"));
            System.IO.Directory.CreateDirectory(destinationDirectory);

            var destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
            if (runConfig.RenameSimilar && File.Exists(destinationFile))
            {
                var suffix = 1;
                var baseName = Path.GetFileNameWithoutExtension(file);
                while (File.Exists(destinationFile))
                {
                    string newFileName = $"{baseName}_({suffix}){extension}";
                    destinationFile = Path.Combine(destinationDirectory, newFileName);
                    suffix++;
                }
            }

            if (runConfig.RenameSimilar || !File.Exists(destinationFile))
            {
                File.Move(file, destinationFile);
            }

            // Update progress and logs
            processedFiles++;
            var progressPercentage = (int)Math.Round(processedFiles / filesPerPercent);
            var logMessage = $"Processed file: {file}";
            progressCallback(progressPercentage, logMessage);
        }

        private static bool IsSupportedFileType(string extension)
        {
            return IsImage(extension) || IsVideo(extension);
        }

        private static bool IsImage(string extension)
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

        private static bool IsVideo(string extension)
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

        private static DateTime GetMediaDateFromExifOrFallback(string filePath)
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
