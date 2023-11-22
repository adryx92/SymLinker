# Symbolic Link Creator

This simple Windows desktop application allows users to create symbolic links easily using a graphical user interface. Symbolic links are a powerful feature in Windows that allow a file or directory to be referenced from another location in the file system.

## Usage

1. **Source Path**: Enter the source path in the "Source Path" text box.

2. **Destination Path**: Enter the destination path in the "Destination Path" text box.

3. **Link Type**: Check the "Directory" checkbox if you want to create a symbolic link to a directory.

4. **Create Link**: Click the "Create Link" button to generate and execute the `mklink` command.

## Implementation Details

The application utilizes the `Process.Start` method to execute the `mklink` command in the background. It captures the exit code of the process to determine the success or failure of the command.

## Requirements

- **Operating System**: Windows (Symbolic links are a feature specific to Windows)

- **.NET Framework**: The application is built using the Windows Presentation Foundation (WPF) and requires the .NET Framework.
