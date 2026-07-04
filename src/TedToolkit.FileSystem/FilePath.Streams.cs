// -----------------------------------------------------------------------
// <copyright file="FilePath.Streams.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides stream factory helpers for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Creates or overwrites the file and returns a writable stream.
    /// </summary>
    /// <returns>A writable file stream.</returns>
    public FileStream Create()
    {
        return File.Create(FullName);
    }

    /// <summary>
    /// Creates or overwrites the file and returns a writable stream that uses the specified buffer size.
    /// </summary>
    /// <param name="bufferSize">The buffer size.</param>
    /// <returns>A writable file stream.</returns>
    public FileStream Create(int bufferSize)
    {
        return File.Create(FullName, bufferSize);
    }

    /// <summary>
    /// Creates or overwrites the file and returns a writable stream that uses the specified buffer size and options.
    /// </summary>
    /// <param name="bufferSize">The buffer size.</param>
    /// <param name="options">The file options.</param>
    /// <returns>A writable file stream.</returns>
    public FileStream Create(int bufferSize, FileOptions options)
    {
        return File.Create(FullName, bufferSize, options);
    }

    /// <summary>
    /// Opens the file with the supplied mode.
    /// </summary>
    /// <param name="mode">The file mode.</param>
    /// <returns>The opened file stream.</returns>
    public FileStream Open(FileMode mode)
    {
        return File.Open(FullName, mode);
    }

    /// <summary>
    /// Opens the file with the supplied mode and access.
    /// </summary>
    /// <param name="mode">The file mode.</param>
    /// <param name="access">The file access mode.</param>
    /// <returns>The opened file stream.</returns>
    public FileStream Open(FileMode mode, FileAccess access)
    {
        return File.Open(FullName, mode, access);
    }

    /// <summary>
    /// Opens the file with the supplied mode, access, and share options.
    /// </summary>
    /// <param name="mode">The file mode.</param>
    /// <param name="access">The file access mode.</param>
    /// <param name="share">The file share mode.</param>
    /// <returns>The opened file stream.</returns>
    public FileStream Open(FileMode mode, FileAccess access, FileShare share)
    {
        return File.Open(FullName, mode, access, share);
    }

    /// <summary>
    /// Opens the file using the supplied file stream options.
    /// </summary>
    /// <param name="options">The file stream options.</param>
    /// <returns>The opened file stream.</returns>
    public FileStream Open(FileStreamOptions options)
    {
        return File.Open(FullName, options);
    }

    /// <summary>
    /// Opens the file for reading.
    /// </summary>
    /// <returns>A readable file stream.</returns>
    public FileStream OpenRead()
    {
        return File.OpenRead(FullName);
    }

    /// <summary>
    /// Opens the file for writing.
    /// </summary>
    /// <returns>A writable file stream.</returns>
    public FileStream OpenWrite()
    {
        return File.OpenWrite(FullName);
    }

    /// <summary>
    /// Opens the file as a text reader.
    /// </summary>
    /// <returns>A stream reader.</returns>
    public StreamReader OpenText()
    {
        return File.OpenText(FullName);
    }

    /// <summary>
    /// Creates or overwrites the file as a text writer.
    /// </summary>
    /// <returns>A stream writer.</returns>
    public StreamWriter CreateText()
    {
        return File.CreateText(FullName);
    }

    /// <summary>
    /// Opens the file as an appending text writer.
    /// </summary>
    /// <returns>A stream writer.</returns>
    public StreamWriter AppendText()
    {
        return File.AppendText(FullName);
    }

    /// <summary>
    /// Opens a file handle for the current file path.
    /// </summary>
    /// <param name="mode">The file mode.</param>
    /// <param name="access">The file access mode.</param>
    /// <param name="share">The file share mode.</param>
    /// <returns>The opened file handle.</returns>
    public Microsoft.Win32.SafeHandles.SafeFileHandle OpenHandle(
        FileMode mode = FileMode.Open,
        FileAccess access = FileAccess.Read,
        FileShare share = FileShare.Read,
        FileOptions options = FileOptions.None,
        long preallocationSize = 0)
    {
        return File.OpenHandle(FullName, mode, access, share, options, preallocationSize);
    }
}
