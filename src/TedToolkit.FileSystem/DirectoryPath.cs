// -----------------------------------------------------------------------
// <copyright file="DirectoryPath.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Represents a directory path value.
/// </summary>
/// <param name="FullName">The full directory path text.</param>
public readonly record struct DirectoryPath(string FullName)
{
    /// <summary>
    /// Creates a directory path value object from a directory info instance.
    /// </summary>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    /// <returns>A directory path value object that uses the directory full name.</returns>
    public static DirectoryPath FromDirectoryInfo(DirectoryInfo directoryInfo)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);
        return new(directoryInfo.FullName);
    }

    /// <summary>
    /// Converts a directory info instance into a directory path value object.
    /// </summary>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    public static implicit operator DirectoryPath(DirectoryInfo directoryInfo)
    {
        return FromDirectoryInfo(directoryInfo);
    }

    /// <summary>
    /// Combines the current directory path with a child directory name.
    /// </summary>
    /// <param name="childDirectoryName">The child directory name.</param>
    /// <returns>A child directory path.</returns>
    public DirectoryPath Combine(string childDirectoryName)
    {
        return new(Path.Combine(FullName, childDirectoryName));
    }

    /// <summary>
    /// Combines the current directory path with a child file name.
    /// </summary>
    /// <param name="fileName">The child file name.</param>
    /// <returns>A child file path.</returns>
    public FilePath Combine(FileName fileName)
    {
        return new(Path.Combine(FullName, fileName.Name));
    }

    /// <summary>
    /// Combines a directory path with a child directory name.
    /// </summary>
    /// <param name="left">The parent directory path.</param>
    /// <param name="right">The child directory name.</param>
    public static DirectoryPath operator /(DirectoryPath left, string right)
    {
        return left.Combine(right);
    }

    /// <summary>
    /// Combines a directory path with a child file name.
    /// </summary>
    /// <param name="left">The parent directory path.</param>
    /// <param name="right">The child file name.</param>
    public static FilePath operator /(DirectoryPath left, FileName right)
    {
        return left.Combine(right);
    }
}
