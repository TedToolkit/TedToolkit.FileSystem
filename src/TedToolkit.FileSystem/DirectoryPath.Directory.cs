// -----------------------------------------------------------------------
// <copyright file="DirectoryPath.Directory.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides directory state and mutation members for <see cref="DirectoryPath" />.
/// </summary>
public readonly partial record struct DirectoryPath
{
    /// <summary>
    /// Gets a value indicating whether the directory exists.
    /// </summary>
    public bool Exists => Directory.Exists(FullName);

    /// <summary>
    /// Gets or sets the directory creation time.
    /// </summary>
    public DateTime CreationTime
    {
        get => Directory.GetCreationTime(FullName);
        set => Directory.SetCreationTime(FullName, value);
    }

    /// <summary>
    /// Gets or sets the directory last write time.
    /// </summary>
    public DateTime LastWriteTime
    {
        get => Directory.GetLastWriteTime(FullName);
        set => Directory.SetLastWriteTime(FullName, value);
    }

    /// <summary>
    /// Gets or sets the directory last access time.
    /// </summary>
    public DateTime LastAccessTime
    {
        get => Directory.GetLastAccessTime(FullName);
        set => Directory.SetLastAccessTime(FullName, value);
    }

    /// <summary>
    /// Gets or sets the directory creation time in Coordinated Universal Time.
    /// </summary>
    public DateTime CreationTimeUtc
    {
        get => Directory.GetCreationTimeUtc(FullName);
        set => Directory.SetCreationTimeUtc(FullName, value);
    }

    /// <summary>
    /// Gets or sets the directory last write time in Coordinated Universal Time.
    /// </summary>
    public DateTime LastWriteTimeUtc
    {
        get => Directory.GetLastWriteTimeUtc(FullName);
        set => Directory.SetLastWriteTimeUtc(FullName, value);
    }

    /// <summary>
    /// Gets or sets the directory last access time in Coordinated Universal Time.
    /// </summary>
    public DateTime LastAccessTimeUtc
    {
        get => Directory.GetLastAccessTimeUtc(FullName);
        set => Directory.SetLastAccessTimeUtc(FullName, value);
    }

    /// <summary>
    /// Gets or sets the file system attributes for the directory.
    /// </summary>
    public FileAttributes Attributes
    {
        get => File.GetAttributes(FullName);
        set => File.SetAttributes(FullName, value);
    }

    /// <summary>
    /// Creates the directory if it does not already exist.
    /// </summary>
    /// <returns>The created directory path.</returns>
    public DirectoryPath Create()
    {
        return new(Directory.CreateDirectory(FullName).FullName);
    }

    /// <summary>
    /// Creates the directory using the specified Unix file mode when the platform supports it.
    /// </summary>
    /// <param name="unixCreateMode">The Unix file mode to apply.</param>
    /// <returns>The created directory path.</returns>
    public DirectoryPath Create(UnixFileMode unixCreateMode)
    {
        return new(Directory.CreateDirectory(FullName, unixCreateMode).FullName);
    }

    /// <summary>
    /// Deletes the directory.
    /// </summary>
    /// <param name="recursive">Whether child content should also be deleted.</param>
    public void Delete(bool recursive = false)
    {
        Directory.Delete(FullName, recursive);
    }

    /// <summary>
    /// Moves the directory to a new destination.
    /// </summary>
    /// <param name="destination">The destination directory path.</param>
    public void MoveTo(DirectoryPath destination)
    {
        Directory.Move(FullName, destination.FullName);
    }

    /// <summary>
    /// Creates a symbolic link at the current directory path that points to the target directory path.
    /// </summary>
    /// <param name="target">The target directory path.</param>
    /// <returns>The created symbolic link path.</returns>
    public DirectoryPath CreateSymbolicLink(DirectoryPath target)
    {
        return new(Directory.CreateSymbolicLink(FullName, target.FullName).FullName);
    }

    /// <summary>
    /// Resolves the current directory path if it is a symbolic link.
    /// </summary>
    /// <param name="returnFinalTarget">Whether the final target should be resolved recursively.</param>
    /// <returns>The resolved target directory path when one exists; otherwise, <see langword="null" />.</returns>
    public DirectoryPath? ResolveLinkTarget(bool returnFinalTarget)
    {
        var fileSystemInfo = Directory.ResolveLinkTarget(FullName, returnFinalTarget);
        return fileSystemInfo is null ? null : new DirectoryPath(fileSystemInfo.FullName);
    }

    /// <summary>
    /// Sets the current working directory to the current directory path.
    /// </summary>
    public void SetCurrentDirectory()
    {
        Directory.SetCurrentDirectory(FullName);
    }
}
