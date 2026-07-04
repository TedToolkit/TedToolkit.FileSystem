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
    /// <remarks>Wraps <see cref="Path.GetFileName(string)" />.</remarks>
    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Path.GetFileName(FullName);
        }
    }

    /// <summary>
    /// Gets the parent directory of the current path when one exists.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetDirectoryName(string)" />.</remarks>
    public DirectoryPath? Parent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Path.GetDirectoryName(FullName) is { } parent ? new DirectoryPath(parent) : default(DirectoryPath?);
        }
    }

    /// <summary>
    /// Gets the root directory of the current path when one exists.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetPathRoot(string)" />.</remarks>
    public DirectoryPath? Root
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Path.GetPathRoot(FullName) is { } root ? new DirectoryPath(root) : default(DirectoryPath?);
        }
    }

    /// <summary>
    /// Gets the extension segment of the current directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetExtension(string)" />.</remarks>
    public string Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Path.GetExtension(FullName);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the current directory path has an extension segment.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.HasExtension(string)" />.</remarks>
    public bool HasExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Path.HasExtension(FullName);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the current directory path is rooted.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.IsPathRooted(string)" />.</remarks>
    public bool IsPathRooted
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Path.IsPathRooted(FullName);
        }
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Gets a value indicating whether the current directory path ends in a directory separator.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.EndsInDirectorySeparator(string)" />.</remarks>
    public bool EndsInDirectorySeparator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Path.EndsInDirectorySeparator(FullName);
        }
    }
#endif

    /// <summary>
    /// Resolves the current directory path to its full absolute path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetFullPath(string)" />.</remarks>
    /// <returns>The resolved absolute directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath GetFullPath()
        {
            return new(Path.GetFullPath(FullName));
        }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Resolves the current directory path to its full absolute path relative to a base directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetFullPath(string,string)" />.</remarks>
    /// <param name="basePath">The base directory path.</param>
    /// <returns>The resolved absolute directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath GetFullPath(DirectoryPath basePath)
        {
            return new(Path.GetFullPath(FullName, basePath.FullName));
        }
#endif

    /// <summary>
    /// Changes the extension segment of the current directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.ChangeExtension(string,string?)" />.</remarks>
    /// <param name="extension">The new extension value.</param>
    /// <returns>A directory path with the changed extension segment.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath ChangeExtension(string? extension)
        {
            return new(Path.ChangeExtension(FullName, extension)!);
        }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Returns the current directory path without any trailing directory separator.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.TrimEndingDirectorySeparator(string)" />.</remarks>
    /// <returns>A directory path without a trailing directory separator.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath TrimEndingDirectorySeparator()
        {
            return new(Path.TrimEndingDirectorySeparator(FullName));
        }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Gets the relative path from the current directory path to another directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetRelativePath(string,string)" />.</remarks>
    /// <param name="target">The target directory path.</param>
    /// <returns>The relative path text from the current directory to the target directory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetRelativePathTo(DirectoryPath target)
        {
            return Path.GetRelativePath(FullName, target.FullName);
        }
#endif
}