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
    /// <remarks>Wraps <see cref="File.Create(string)" />.</remarks>
    /// <returns>A writable file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream Create()
    {
        return File.Create(FullName);
    }

    /// <summary>
    /// Creates or overwrites the file and returns a writable stream that uses the specified buffer size.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Create(string,int)" />.</remarks>
    /// <param name="bufferSize">The buffer size.</param>
    /// <returns>A writable file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream Create(int bufferSize)
    {
        return File.Create(FullName, bufferSize);
    }

    /// <summary>
    /// Creates or overwrites the file and returns a writable stream that uses the specified buffer size and options.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Create(string,int,FileOptions)" />.</remarks>
    /// <param name="bufferSize">The buffer size.</param>
    /// <param name="options">The file options.</param>
    /// <returns>A writable file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream Create(int bufferSize, FileOptions options)
    {
        return File.Create(FullName, bufferSize, options);
    }

    /// <summary>
    /// Opens the file with the supplied mode.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Open(string,FileMode)" />.</remarks>
    /// <param name="mode">The file mode.</param>
    /// <returns>The opened file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream Open(FileMode mode)
    {
        return File.Open(FullName, mode);
    }

    /// <summary>
    /// Opens the file with the supplied mode and access.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Open(string,FileMode,FileAccess)" />.</remarks>
    /// <param name="mode">The file mode.</param>
    /// <param name="access">The file access mode.</param>
    /// <returns>The opened file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream Open(FileMode mode, FileAccess access)
    {
        return File.Open(FullName, mode, access);
    }

    /// <summary>
    /// Opens the file with the supplied mode, access, and share options.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Open(string,FileMode,FileAccess,FileShare)" />.</remarks>
    /// <param name="mode">The file mode.</param>
    /// <param name="access">The file access mode.</param>
    /// <param name="share">The file share mode.</param>
    /// <returns>The opened file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream Open(FileMode mode, FileAccess access, FileShare share)
    {
        return File.Open(FullName, mode, access, share);
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Opens the file using the supplied file stream options.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Open(string,FileStreamOptions)" />.</remarks>
    /// <param name="options">The file stream options.</param>
    /// <returns>The opened file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream Open(FileStreamOptions options)
    {
        return File.Open(FullName, options);
    }
#endif

#if NET6_0_OR_GREATER
    /// <summary>
    /// Opens a file handle for the current file path.
    /// </summary>
    /// <remarks>
    /// Wraps <c>File.OpenHandle(string, FileMode, FileAccess, FileShare, FileOptions, long)</c>.
    /// </remarks>
    /// <param name="mode">The file mode.</param>
    /// <param name="access">The file access mode.</param>
    /// <param name="share">The file share mode.</param>
    /// <param name="options">The file options.</param>
    /// <param name="preallocationSize">The preallocated size, in bytes.</param>
    /// <returns>The opened file handle.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Microsoft.Win32.SafeHandles.SafeFileHandle OpenHandle(
        FileMode mode = FileMode.Open,
        FileAccess access = FileAccess.Read,
        FileShare share = FileShare.Read,
        FileOptions options = FileOptions.None,
        long preallocationSize = 0)
    {
        return File.OpenHandle(FullName, mode, access, share, options, preallocationSize);
    }
#endif

    /// <summary>
    /// Opens the file for reading.
    /// </summary>
    /// <remarks>Wraps <see cref="File.OpenRead(string)" />.</remarks>
    /// <returns>A readable file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream OpenRead()
    {
        return File.OpenRead(FullName);
    }

    /// <summary>
    /// Opens the file for writing.
    /// </summary>
    /// <remarks>Wraps <see cref="File.OpenWrite(string)" />.</remarks>
    /// <returns>A writable file stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileStream OpenWrite()
    {
        return File.OpenWrite(FullName);
    }

    /// <summary>
    /// Opens the file as a text reader.
    /// </summary>
    /// <remarks>Wraps <see cref="File.OpenText(string)" />.</remarks>
    /// <returns>A stream reader.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StreamReader OpenText()
    {
        return File.OpenText(FullName);
    }

    /// <summary>
    /// Creates or overwrites the file as a text writer.
    /// </summary>
    /// <remarks>Wraps <see cref="File.CreateText(string)" />.</remarks>
    /// <returns>A stream writer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StreamWriter CreateText()
    {
        return File.CreateText(FullName);
    }

    /// <summary>
    /// Opens the file as an appending text writer.
    /// </summary>
    /// <remarks>Wraps <see cref="File.AppendText(string)" />.</remarks>
    /// <returns>A stream writer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public StreamWriter AppendText()
    {
        return File.AppendText(FullName);
    }
}