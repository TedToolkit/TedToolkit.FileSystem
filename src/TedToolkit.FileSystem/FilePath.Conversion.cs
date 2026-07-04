// -----------------------------------------------------------------------
// <copyright file="FilePath.Conversion.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides conversion and transformation helpers for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Converts the path value object into a file info instance.
    /// </summary>
    /// <returns>A file info instance for the current path.</returns>
    public FileInfo ToFileInfo()
    {
        return new(FullName);
    }

    /// <summary>
    /// Gets the absolute file path for the current path text.
    /// </summary>
    /// <returns>An absolute file path value.</returns>
    public FilePath GetFullPath()
    {
        return new(Path.GetFullPath(FullName));
    }

    /// <summary>
    /// Gets the absolute file path for the current path text using the supplied base directory.
    /// </summary>
    /// <param name="basePath">The base directory to resolve against.</param>
    /// <returns>An absolute file path value.</returns>
    public FilePath GetFullPath(DirectoryPath basePath)
    {
        return new(Path.GetFullPath(FullName, basePath.FullName));
    }

    /// <summary>
    /// Changes the file extension.
    /// </summary>
    /// <param name="extension">The new extension value.</param>
    /// <returns>A file path with the updated extension.</returns>
    public FilePath ChangeExtension(string? extension)
    {
        return new(Path.ChangeExtension(FullName, extension)!);
    }

    /// <summary>
    /// Returns a file path with the supplied file name in the current parent directory.
    /// </summary>
    /// <param name="fileName">The replacement file name.</param>
    /// <returns>A file path with the replacement file name.</returns>
    public FilePath WithFileName(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        return ParentDirectory is { } directoryPath
            ? new(Path.Combine(directoryPath.FullName, fileName))
            : new(fileName);
    }

    /// <summary>
    /// Returns a file path with the supplied extension.
    /// </summary>
    /// <param name="extension">The replacement extension.</param>
    /// <returns>A file path with the replacement extension.</returns>
    public FilePath WithExtension(string? extension)
    {
        return ChangeExtension(extension);
    }

    /// <summary>
    /// Gets the path from the supplied base directory to the current file path.
    /// </summary>
    /// <param name="relativeTo">The base directory path.</param>
    /// <returns>A relative file path value.</returns>
    public FilePath GetRelativePath(DirectoryPath relativeTo)
    {
        return new(Path.GetRelativePath(relativeTo.FullName, FullName));
    }

    /// <summary>
    /// Gets the path from the supplied base directory string to the current file path.
    /// </summary>
    /// <param name="relativeTo">The base directory path string.</param>
    /// <returns>A relative file path value.</returns>
    public FilePath GetRelativePath(string relativeTo)
    {
        return new(Path.GetRelativePath(relativeTo, FullName));
    }
}
