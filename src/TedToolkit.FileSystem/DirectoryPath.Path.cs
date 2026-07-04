// -----------------------------------------------------------------------
// <copyright file="DirectoryPath.Path.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides path metadata and path transformation members for <see cref="DirectoryPath" />.
/// </summary>
public readonly partial record struct DirectoryPath
{
    /// <summary>
    /// Gets the directory name segment from the current path.
    /// </summary>
    public string Name => Path.GetFileName(GetPathWithoutTrailingSeparator());

    /// <summary>
    /// Gets the parent directory of the current path when one exists.
    /// </summary>
    public DirectoryPath? Parent
    {
        get
        {
            var parent = Path.GetDirectoryName(GetPathWithoutTrailingSeparator());
            return parent is null ? null : new DirectoryPath(parent);
        }
    }

    /// <summary>
    /// Gets the root directory of the current path when one exists.
    /// </summary>
    public DirectoryPath? Root
    {
        get
        {
            var root = Path.GetPathRoot(FullName);
            return root is null ? null : new DirectoryPath(root);
        }
    }

    /// <summary>
    /// Gets the extension segment of the current directory path.
    /// </summary>
    public string Extension => Path.GetExtension(GetPathWithoutTrailingSeparator());

    /// <summary>
    /// Gets a value indicating whether the current directory path has an extension segment.
    /// </summary>
    public bool HasExtension => Path.HasExtension(GetPathWithoutTrailingSeparator());

    /// <summary>
    /// Gets a value indicating whether the current directory path is rooted.
    /// </summary>
    public bool IsPathRooted => Path.IsPathRooted(FullName);

    /// <summary>
    /// Gets a value indicating whether the current directory path ends in a directory separator.
    /// </summary>
    public bool EndsInDirectorySeparator => Path.EndsInDirectorySeparator(FullName);

    /// <summary>
    /// Resolves the current directory path to its full absolute path.
    /// </summary>
    /// <returns>The resolved absolute directory path.</returns>
    public DirectoryPath GetFullPath()
    {
        return new(Path.GetFullPath(FullName));
    }

    /// <summary>
    /// Resolves the current directory path to its full absolute path relative to a base directory path.
    /// </summary>
    /// <param name="basePath">The base directory path.</param>
    /// <returns>The resolved absolute directory path.</returns>
    public DirectoryPath GetFullPath(DirectoryPath basePath)
    {
        return new(Path.GetFullPath(FullName, basePath.FullName));
    }

    /// <summary>
    /// Returns the current directory path without any trailing directory separator.
    /// </summary>
    /// <returns>A directory path without a trailing directory separator.</returns>
    public DirectoryPath TrimEndingDirectorySeparator()
    {
        return new(Path.TrimEndingDirectorySeparator(FullName));
    }

    /// <summary>
    /// Changes the extension segment of the current directory path.
    /// </summary>
    /// <param name="extension">The new extension value.</param>
    /// <returns>A directory path with the changed extension segment.</returns>
    public DirectoryPath ChangeExtension(string? extension)
    {
        return new(Path.ChangeExtension(GetPathWithoutTrailingSeparator(), extension)!);
    }

    /// <summary>
    /// Gets the relative path from the current directory path to another directory path.
    /// </summary>
    /// <param name="target">The target directory path.</param>
    /// <returns>The relative path text from the current directory to the target directory.</returns>
    public string GetRelativePathTo(DirectoryPath target)
    {
        return Path.GetRelativePath(FullName, target.FullName);
    }

    private string GetPathWithoutTrailingSeparator()
    {
        return Path.TrimEndingDirectorySeparator(FullName);
    }
}
