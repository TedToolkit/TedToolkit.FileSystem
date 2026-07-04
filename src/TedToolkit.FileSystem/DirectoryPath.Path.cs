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
    /// <remarks>Wraps <see cref="Path.GetFileName(string)" /> after trimming any trailing directory separator.</remarks>
    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetFileName(GetPathWithoutTrailingSeparator());
    }

    /// <summary>
    /// Gets the parent directory of the current path when one exists.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetDirectoryName(string)" /> after trimming any trailing directory separator.</remarks>
    public DirectoryPath? Parent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetDirectoryName(GetPathWithoutTrailingSeparator()) is { } parent ? new DirectoryPath(parent) : null;
    }

    /// <summary>
    /// Gets the root directory of the current path when one exists.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetPathRoot(string)" />.</remarks>
    public DirectoryPath? Root
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetPathRoot(FullName) is { } root ? new DirectoryPath(root) : null;
    }

    /// <summary>
    /// Gets the extension segment of the current directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetExtension(string)" /> after trimming any trailing directory separator.</remarks>
    public string Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetExtension(GetPathWithoutTrailingSeparator());
    }

    /// <summary>
    /// Gets a value indicating whether the current directory path has an extension segment.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.HasExtension(string)" /> after trimming any trailing directory separator.</remarks>
    public bool HasExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.HasExtension(GetPathWithoutTrailingSeparator());
    }

    /// <summary>
    /// Gets a value indicating whether the current directory path is rooted.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.IsPathRooted(string)" />.</remarks>
    public bool IsPathRooted
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.IsPathRooted(FullName);
    }

    /// <summary>
    /// Gets a value indicating whether the current directory path ends in a directory separator.
    /// </summary>
    /// <remarks>Determines whether the current path ends in a directory separator.</remarks>
    public bool EndsInDirectorySeparator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
#if NET6_0_OR_GREATER
            return Path.EndsInDirectorySeparator(FullName);
#else
            return FullName.Length > 0 && (FullName[FullName.Length - 1] == Path.DirectorySeparatorChar || FullName[FullName.Length - 1] == Path.AltDirectorySeparatorChar);
#endif
        }
    }

    /// <summary>
    /// Resolves the current directory path to its full absolute path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetFullPath(string)" />.</remarks>
    /// <returns>The resolved absolute directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath GetFullPath()
        => new(Path.GetFullPath(FullName));

    /// <summary>
    /// Resolves the current directory path to its full absolute path relative to a base directory path.
    /// </summary>
    /// <remarks>Resolves the current path against the supplied base directory.</remarks>
    /// <param name="basePath">The base directory path.</param>
    /// <returns>The resolved absolute directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath GetFullPath(DirectoryPath basePath)
    {
#if NET6_0_OR_GREATER
        return new(Path.GetFullPath(FullName, basePath.FullName));
#else
        return new(Path.GetFullPath(Path.Combine(basePath.FullName, FullName)));
#endif
    }

    /// <summary>
    /// Changes the extension segment of the current directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.ChangeExtension(string,string?)" /> after trimming any trailing directory separator.</remarks>
    /// <param name="extension">The new extension value.</param>
    /// <returns>A directory path with the changed extension segment.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath ChangeExtension(string? extension)
        => new(Path.ChangeExtension(GetPathWithoutTrailingSeparator(), extension)!);

    /// <summary>
    /// Returns the current directory path without any trailing directory separator.
    /// </summary>
    /// <remarks>Returns the current path without any trailing directory separator.</remarks>
    /// <returns>A directory path without a trailing directory separator.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath TrimEndingDirectorySeparator()
    {
#if NET6_0_OR_GREATER
        return new(Path.TrimEndingDirectorySeparator(FullName));
#else
        return new(Path.GetPathRoot(FullName) is { } root ? FullName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length >= root.Length ? FullName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) : root : FullName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
#endif
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1
    /// <summary>
    /// Gets the relative path from the current directory path to another directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetRelativePath(string,string)" />.</remarks>
    /// <param name="target">The target directory path.</param>
    /// <returns>The relative path text from the current directory to the target directory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetRelativePathTo(DirectoryPath target)
        => Path.GetRelativePath(FullName, target.FullName);
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetPathWithoutTrailingSeparator()
        => TrimEndingDirectorySeparator().FullName;
}
