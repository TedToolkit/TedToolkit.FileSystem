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
    /// <remarks>Creates a new <see cref="FileInfo" /> via <see cref="FileInfo.FileInfo(string)" />.</remarks>
    /// <returns>A file info instance for the current path.</returns>
    public FileInfo ToFileInfo()
    {
        return new(FullName);
    }

    /// <summary>
    /// Gets the absolute file path for the current path text.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetFullPath(string)" />.</remarks>
    /// <returns>An absolute file path value.</returns>
    public FilePath GetFullPath()
    {
        return new(Path.GetFullPath(FullName));
    }

    /// <summary>
    /// Gets the absolute file path for the current path text using the supplied base directory.
    /// </summary>
    /// <remarks>Resolves the current path against the supplied base directory.</remarks>
    /// <param name="basePath">The base directory to resolve against.</param>
    /// <returns>An absolute file path value.</returns>
    public FilePath GetFullPath(DirectoryPath basePath)
    {
        return new(Compatibility.GetFullPath(FullName, basePath.FullName));
    }

    /// <summary>
    /// Changes the file extension.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.ChangeExtension(string,string?)" />.</remarks>
    /// <param name="extension">The new extension value.</param>
    /// <returns>A file path with the updated extension.</returns>
    public FilePath ChangeExtension(string? extension)
    {
        return new(Path.ChangeExtension(FullName, extension)!);
    }

    /// <summary>
    /// Gets the path from the supplied base directory to the current file path.
    /// </summary>
    /// <remarks>Computes the relative path from the supplied base directory to the current file path.</remarks>
    /// <param name="relativeTo">The base directory path.</param>
    /// <returns>A relative file path value.</returns>
    public FilePath GetRelativePath(DirectoryPath relativeTo)
    {
        return new(Compatibility.GetRelativePath(relativeTo.FullName, FullName));
    }

    /// <summary>
    /// Gets the path from the supplied base directory string to the current file path.
    /// </summary>
    /// <remarks>Computes the relative path from the supplied base directory string to the current file path.</remarks>
    /// <param name="relativeTo">The base directory path string.</param>
    /// <returns>A relative file path value.</returns>
    public FilePath GetRelativePath(string relativeTo)
    {
        return new(Compatibility.GetRelativePath(relativeTo, FullName));
    }
}
