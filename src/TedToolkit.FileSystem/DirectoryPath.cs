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
/// <param name="FullName">The full directory path text stored by this value object.</param>
public readonly partial record struct DirectoryPath(string FullName)
{
    /// <summary>
    /// Creates a directory path value object from a directory info instance.
    /// </summary>
    /// <remarks>Uses the <see cref="FileSystemInfo.FullName" /> value from the supplied <see cref="DirectoryInfo" /> instance.</remarks>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="directoryInfo" /> is <see langword="null" />.</exception>
    /// <returns>A directory path value object that uses the directory full name.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DirectoryPath FromDirectoryInfo(DirectoryInfo directoryInfo)
    {
        return new((directoryInfo ?? throw new ArgumentNullException(nameof(directoryInfo))).FullName);
    }

    /// <summary>
    /// Converts a directory info instance into a directory path value object.
    /// </summary>
    /// <remarks>Uses the <see cref="FileSystemInfo.FullName" /> value from the supplied <see cref="DirectoryInfo" /> instance.</remarks>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator DirectoryPath(DirectoryInfo directoryInfo)
    {
        return FromDirectoryInfo(directoryInfo);
    }

    /// <summary>
    /// Combines the current directory path with a child directory name.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.Combine(string,string)" />.</remarks>
    /// <param name="childDirectoryName">The child directory name.</param>
    /// <returns>A child directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath Combine(string childDirectoryName)
    {
        return new(Path.Combine(FullName, childDirectoryName));
    }

    /// <summary>
    /// Combines the current directory path with a child file name.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.Combine(string,string)" /> with <see cref="FileName.Name" />.</remarks>
    /// <param name="fileName">The child file name.</param>
    /// <returns>A child file path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath Combine(FileName fileName)
    {
        return new(Path.Combine(FullName, fileName.Name));
    }

    /// <summary>
    /// Combines a directory path with a child directory name.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.Combine(string,string)" />.</remarks>
    /// <param name="left">The parent directory path.</param>
    /// <param name="right">The child directory name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DirectoryPath operator /(DirectoryPath left, string right)
    {
        return new(Path.Combine(left.FullName, right));
    }

    /// <summary>
    /// Combines a directory path with a child file name.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.Combine(string,string)" /> with <see cref="FileName.Name" />.</remarks>
    /// <param name="left">The parent directory path.</param>
    /// <param name="right">The child file name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FilePath operator /(DirectoryPath left, FileName right)
    {
        return new(Path.Combine(left.FullName, right.Name));
    }

    /// <summary>
    /// Returns the raw full directory path text.
    /// </summary>
    /// <remarks>Returns the stored <c>FullName</c> value directly without calling an additional BCL API.</remarks>
    /// <returns>The raw full directory path text.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return FullName;
    }
}