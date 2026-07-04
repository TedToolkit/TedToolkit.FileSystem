// -----------------------------------------------------------------------
// <copyright file="FilePath.Properties.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides derived path and metadata properties for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Gets the file name portion of the path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetFileName(string)" />.</remarks>
    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetFileName(FullName);
    }

    /// <summary>
    /// Gets the file name portion of the path without its extension.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetFileNameWithoutExtension(string)" />.</remarks>
    public string NameWithoutExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetFileNameWithoutExtension(FullName);
    }

    /// <summary>
    /// Gets the file name value object for the current path.
    /// </summary>
    /// <remarks>Builds a <see cref="FileName" /> from <see cref="Name" />, which wraps <see cref="Path.GetFileName(string)" />.</remarks>
    public FileName FileName
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Name);
    }

    /// <summary>
    /// Gets the file extension portion of the path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetExtension(string)" />.</remarks>
    public string? Extension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetExtension(FullName);
    }

    /// <summary>
    /// Gets the parent directory path when one is available.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetDirectoryName(string)" />.</remarks>
    public DirectoryPath? ParentDirectory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetDirectoryName(FullName) is { } directoryName ? new DirectoryPath(directoryName) : default(DirectoryPath?);
    }

    /// <summary>
    /// Gets the root directory information for the current path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetPathRoot(string)" />.</remarks>
    public string Root
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.GetPathRoot(FullName) ?? "";
    }

    /// <summary>
    /// Gets a value indicating whether the file path has an extension.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.HasExtension(string)" />.</remarks>
    public bool HasExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.HasExtension(FullName);
    }

    /// <summary>
    /// Gets a value indicating whether the path string contains a root.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.IsPathRooted(string)" />.</remarks>
    public bool IsPathRooted
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Path.IsPathRooted(FullName);
    }

    /// <summary>
    /// Gets a value indicating whether the path is fully qualified.
    /// </summary>
    /// <remarks>Determines whether the current path is fully qualified.</remarks>
    public bool IsFullyQualified
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
#if NET6_0_OR_GREATER
            return Path.IsPathFullyQualified(FullName);
#else
            return Path.IsPathRooted(FullName) && !string.IsNullOrEmpty(Path.GetPathRoot(FullName)) && FullName.Length > Path.GetPathRoot(FullName)!.Length;
#endif
        }
    }

    /// <summary>
    /// Gets a value indicating whether the file exists.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Exists(string)" />.</remarks>
    public bool Exists
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.Exists(FullName);
    }

    /// <summary>
    /// Gets the file length in bytes.
    /// </summary>
    /// <remarks>Reads <see cref="FileInfo.Length" /> from the <see cref="FileInfo" /> created by <see cref="ToFileInfo()" />.</remarks>
    public long Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ToFileInfo().Length;
    }

    /// <summary>
    /// Gets or sets the file creation time in local time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetCreationTime(string)" />.
    /// The setter wraps <see cref="File.SetCreationTime(string,System.DateTime)" />.
    /// </remarks>
    public DateTime CreationTime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.GetCreationTime(FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => File.SetCreationTime(FullName, value);
    }

    /// <summary>
    /// Gets or sets the file creation time in UTC.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetCreationTimeUtc(string)" />.
    /// The setter wraps <see cref="File.SetCreationTimeUtc(string,System.DateTime)" />.
    /// </remarks>
    public DateTime CreationTimeUtc
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.GetCreationTimeUtc(FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => File.SetCreationTimeUtc(FullName, value);
    }

    /// <summary>
    /// Gets or sets the file last write time in local time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetLastWriteTime(string)" />.
    /// The setter wraps <see cref="File.SetLastWriteTime(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastWriteTime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.GetLastWriteTime(FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => File.SetLastWriteTime(FullName, value);
    }

    /// <summary>
    /// Gets or sets the file last write time in UTC.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetLastWriteTimeUtc(string)" />.
    /// The setter wraps <see cref="File.SetLastWriteTimeUtc(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastWriteTimeUtc
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.GetLastWriteTimeUtc(FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => File.SetLastWriteTimeUtc(FullName, value);
    }

    /// <summary>
    /// Gets or sets the file last access time in local time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetLastAccessTime(string)" />.
    /// The setter wraps <see cref="File.SetLastAccessTime(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastAccessTime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.GetLastAccessTime(FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => File.SetLastAccessTime(FullName, value);
    }

    /// <summary>
    /// Gets or sets the file last access time in UTC.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetLastAccessTimeUtc(string)" />.
    /// The setter wraps <see cref="File.SetLastAccessTimeUtc(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastAccessTimeUtc
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.GetLastAccessTimeUtc(FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => File.SetLastAccessTimeUtc(FullName, value);
    }

    /// <summary>
    /// Gets or sets the file attributes.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetAttributes(string)" />.
    /// The setter wraps <see cref="File.SetAttributes(string,System.IO.FileAttributes)" />.
    /// </remarks>
    public FileAttributes Attributes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => File.GetAttributes(FullName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => File.SetAttributes(FullName, value);
    }
}
