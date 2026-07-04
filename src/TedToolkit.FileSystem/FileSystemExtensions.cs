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
    /// Converts a string value to a file name value object.
    /// </summary>
    /// <param name="value">The file name text.</param>
    /// <returns>A file name value object.</returns>
    public static FileName ToFileName(this string value)
    {
        return new(value);
    }

    /// <summary>
    /// Converts a base file name and extension to a file name value object.
    /// </summary>
    /// <param name="value">The base file name text.</param>
    /// <param name="extension">The file extension without a leading period.</param>
    /// <returns>A file name value object.</returns>
    public static FileName ToFileName(this string value, string extension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);
        return new($"{value}.{extension}");
    }

    /// <summary>
    /// Converts a file name string to a file name value object with the supplied extension.
    /// </summary>
    /// <param name="value">The file name text.</param>
    /// <param name="extension">The file extension without a leading period.</param>
    /// <returns>A file name value object that uses the supplied extension.</returns>
    public static FileName ToFileNameWithExtension(this string value, string extension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);
        return new(Path.ChangeExtension(value, $".{extension}"));
    }

    /// <summary>
    /// Converts a directory info instance to a directory path value object.
    /// </summary>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    /// <returns>A directory path value object.</returns>
    public static DirectoryPath ToPath(this DirectoryInfo directoryInfo)
    {
        return directoryInfo;
    }

    /// <summary>
    /// Converts a file info instance to a file path value object.
    /// </summary>
    /// <param name="fileInfo">The file info instance to convert.</param>
    /// <returns>A file path value object.</returns>
    public static FilePath ToPath(this FileInfo fileInfo)
    {
        return fileInfo;
    }
}
