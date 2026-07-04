// -----------------------------------------------------------------------
// <copyright file="FileSystemExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides extension methods for file system value objects.
/// </summary>
public static class FileSystemExtensions
{
    /// <summary>
    /// Wraps a string value as a file name value object.
    /// </summary>
    /// <remarks>Constructs <see cref="FileName" /> directly and does not wrap an additional BCL API.</remarks>
    /// <param name="value">The file name text.</param>
    /// <returns>A file name value object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileName AsFileName(this string value)
    {
        return new(value);
    }

    /// <summary>
    /// Wraps a base file name and extension as a file name value object.
    /// </summary>
    /// <remarks>Constructs <see cref="FileName" /> using <see cref="Path.ChangeExtension(string,string?)" /> semantics.</remarks>
    /// <param name="value">The base file name text.</param>
    /// <param name="extension">The extension to apply.</param>
    /// <returns>A file name value object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileName AsFileName(this string value, string? extension)
    {
        return new(value, extension);
    }

    /// <summary>
    /// Converts a directory info instance to a directory path value object.
    /// </summary>
    /// <remarks>Uses the <see cref="FileSystemInfo.FullName" /> value from the supplied <see cref="DirectoryInfo" /> instance.</remarks>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="directoryInfo" /> is <see langword="null" />.</exception>
    /// <returns>A directory path value object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DirectoryPath ToPath(this DirectoryInfo directoryInfo)
    {
        return DirectoryPath.FromDirectoryInfo(directoryInfo);
    }

    /// <summary>
    /// Converts a file info instance to a file path value object.
    /// </summary>
    /// <remarks>Uses the <see cref="FileSystemInfo.FullName" /> value from the supplied <see cref="FileInfo" /> instance.</remarks>
    /// <param name="fileInfo">The file info instance to convert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fileInfo" /> is <see langword="null" />.</exception>
    /// <returns>A file path value object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FilePath ToPath(this FileInfo fileInfo)
    {
        return FilePath.FromFileInfo(fileInfo);
    }
}