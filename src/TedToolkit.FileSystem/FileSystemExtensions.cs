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
    public static FileName AsFileName(this string value)
    {
        return new(value);
    }

    /// <summary>
    /// Converts a directory info instance to a directory path value object.
    /// </summary>
    /// <remarks>Delegates to the implicit <see cref="DirectoryPath" /> conversion, which uses the <see cref="DirectoryInfo.FullName" /> value.</remarks>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    /// <returns>A directory path value object.</returns>
    public static DirectoryPath ToPath(this DirectoryInfo directoryInfo)
    {
        return directoryInfo;
    }

    /// <summary>
    /// Converts a file info instance to a file path value object.
    /// </summary>
    /// <remarks>Delegates to the implicit <see cref="FilePath" /> conversion, which uses the <see cref="FileInfo.FullName" /> value.</remarks>
    /// <param name="fileInfo">The file info instance to convert.</param>
    /// <returns>A file path value object.</returns>
    public static FilePath ToPath(this FileInfo fileInfo)
    {
        return fileInfo;
    }
}
