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
    /// <remarks>Wraps <see cref="Directory.Exists(string)" />.</remarks>
    public bool Exists
    {
        get
        {
            return Directory.Exists(FullName);
        }
    }

    /// <summary>
    /// Gets or sets the directory creation time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="Directory.GetCreationTime(string)" />.
    /// The setter wraps <see cref="Directory.SetCreationTime(string,System.DateTime)" />.
    /// </remarks>
    public DateTime CreationTime
    {
        get
        {
            return Directory.GetCreationTime(FullName);
        }

        set
        {
            Directory.SetCreationTime(FullName, value);
        }
    }

    /// <summary>
    /// Gets or sets the directory last write time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="Directory.GetLastWriteTime(string)" />.
    /// The setter wraps <see cref="Directory.SetLastWriteTime(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastWriteTime
    {
        get
        {
            return Directory.GetLastWriteTime(FullName);
        }

        set
        {
            Directory.SetLastWriteTime(FullName, value);
        }
    }

    /// <summary>
    /// Gets or sets the directory last access time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="Directory.GetLastAccessTime(string)" />.
    /// The setter wraps <see cref="Directory.SetLastAccessTime(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastAccessTime
    {
        get
        {
            return Directory.GetLastAccessTime(FullName);
        }

        set
        {
            Directory.SetLastAccessTime(FullName, value);
        }
    }

    /// <summary>
    /// Gets or sets the directory creation time in Coordinated Universal Time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="Directory.GetCreationTimeUtc(string)" />.
    /// The setter wraps <see cref="Directory.SetCreationTimeUtc(string,System.DateTime)" />.
    /// </remarks>
    public DateTime CreationTimeUtc
    {
        get
        {
            return Directory.GetCreationTimeUtc(FullName);
        }

        set
        {
            Directory.SetCreationTimeUtc(FullName, value);
        }
    }

    /// <summary>
    /// Gets or sets the directory last write time in Coordinated Universal Time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="Directory.GetLastWriteTimeUtc(string)" />.
    /// The setter wraps <see cref="Directory.SetLastWriteTimeUtc(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastWriteTimeUtc
    {
        get
        {
            return Directory.GetLastWriteTimeUtc(FullName);
        }

        set
        {
            Directory.SetLastWriteTimeUtc(FullName, value);
        }
    }

    /// <summary>
    /// Gets or sets the directory last access time in Coordinated Universal Time.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="Directory.GetLastAccessTimeUtc(string)" />.
    /// The setter wraps <see cref="Directory.SetLastAccessTimeUtc(string,System.DateTime)" />.
    /// </remarks>
    public DateTime LastAccessTimeUtc
    {
        get
        {
            return Directory.GetLastAccessTimeUtc(FullName);
        }

        set
        {
            Directory.SetLastAccessTimeUtc(FullName, value);
        }
    }

    /// <summary>
    /// Gets or sets the file system attributes for the directory.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetAttributes(string)" />.
    /// The setter wraps <see cref="File.SetAttributes(string,System.IO.FileAttributes)" />.
    /// </remarks>
    public FileAttributes Attributes
    {
        get
        {
            return File.GetAttributes(FullName);
        }

        set
        {
            File.SetAttributes(FullName, value);
        }
    }

    /// <summary>
    /// Creates the directory if it does not already exist.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.CreateDirectory(string)" />.</remarks>
    /// <returns>The created directory path.</returns>
    public DirectoryPath Create()
    {
        return new(Directory.CreateDirectory(FullName).FullName);
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// Creates the directory using the specified Unix file mode when the platform supports it.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.CreateDirectory(string,System.IO.UnixFileMode)" />.</remarks>
    /// <param name="unixCreateMode">The Unix file mode to apply.</param>
    /// <returns>The created directory path.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown when Unix file modes are requested on Windows.</exception>
    public DirectoryPath Create(UnixFileMode unixCreateMode)
    {
        if (OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Unix file modes are not supported on Windows.");
        }

        return new(Directory.CreateDirectory(FullName, unixCreateMode).FullName);
    }

    /// <summary>
    /// Creates a symbolic link at the current directory path that points to the target directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.CreateSymbolicLink(string,string)" />.</remarks>
    /// <param name="target">The target directory path.</param>
    /// <returns>The created symbolic link path.</returns>
    public DirectoryPath CreateSymbolicLink(DirectoryPath target)
    {
        return new(Directory.CreateSymbolicLink(FullName, target.FullName).FullName);
    }

    /// <summary>
    /// Resolves the current directory path if it is a symbolic link.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.ResolveLinkTarget(string,bool)" />.</remarks>
    /// <param name="returnFinalTarget">Whether the final target should be resolved recursively.</param>
    /// <returns>The resolved target directory path when one exists; otherwise, <see langword="null" />.</returns>
    public DirectoryPath? ResolveLinkTarget(bool returnFinalTarget)
    {
        var fileSystemInfo = Directory.ResolveLinkTarget(FullName, returnFinalTarget);
        if (fileSystemInfo is null)
        {
            return null;
        }

        return new DirectoryPath(fileSystemInfo.FullName);
    }
#endif

    /// <summary>
    /// Deletes the directory.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.Delete(string,bool)" />.</remarks>
    /// <param name="recursive">Whether child content should also be deleted.</param>
    public void Delete(bool recursive = false)
    {
        Directory.Delete(FullName, recursive);
    }

    /// <summary>
    /// Moves the directory to a new destination.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.Move(string,string)" />.</remarks>
    /// <param name="destination">The destination directory path.</param>
    public void MoveTo(DirectoryPath destination)
    {
        Directory.Move(FullName, destination.FullName);
    }

    /// <summary>
    /// Sets the current working directory to the current directory path.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.SetCurrentDirectory(string)" />.</remarks>
    public void SetCurrentDirectory()
    {
        Directory.SetCurrentDirectory(FullName);
    }
}
