# Media Organizer

This is a simple media organizer application that helps you organize your media files by date. It allows you to specify a source directory containing image and video files, and it organizes them into subdirectories based on their creation date.

## Features

- Organizes image and video files into subdirectories based on creation date.
- Supports various image formats: JPG, JPEG, PNG, GIF, BMP, TIFF.
- Supports various video formats: MP4, AVI, WMV, MOV.
- Handles media files without metadata by using the file creation date as a fallback.
- Renames files with the same name to avoid conflicts.
- Removes empty directories after organizing files.
- Provides progress updates and logs during the organization process.
- Allows cancellation of the organization process.

## Prerequisites

- .NET Framework 4.7.2 or later

## Installation

1. Clone or download the repository from GitHub: [Media Organizer Repository](https://github.com/aabir121/MediaOrganizer)
2. Open the solution in Visual Studio.
3. Build the solution to restore NuGet packages and compile the application.

## Usage

1. Launch the Media Organizer application.
2. Click the "Choose Directory" button to select the source directory containing the media files you want to organize.
3. Enable the desired options:
    - "Rename Similar": Renames files with the same name to avoid conflicts.
    - "Remove Empty Directories": Deletes empty directories after organizing files.
4. Click the "Start" button to begin the organization process.
5. The progress bar will display the progress of the organization process, and the log window will show the files being processed.
6. Once the organization process is complete, a message box will inform you of the completion.
7. You can click the "Cancel" button at any time to stop the organization process.

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

- This application uses the [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet) library to extract metadata from media files.

## Contributing

Contributions are welcome! Please follow these guidelines when contributing to the project:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Commit your changes and push them to your fork.
4. Submit a pull request explaining your changes.

## Authors

- [Aabir Hassan]([https://github.com/your-username](https://github.com/aabir121))

## About

This Media Organizer application was developed to assist in organizing media files based on their creation date. It provides a simple and straightforward way to keep your media library organized. Feel free to customize and enhance it to suit your specific needs.
