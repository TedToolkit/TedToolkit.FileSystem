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
        get
        {
            return Path.GetFileName(FullName);
        }
    }

    /// <summary>
    /// Gets the file name portion of the path without its extension.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetFileNameWithoutExtension(string)" />.</remarks>
    public string NameWithoutExtension
    {
        get
        {
            return Path.GetFileNameWithoutExtension(FullName);
        }
    }

    /// <summary>
    /// Gets the file name value object for the current path.
    /// </summary>
    /// <remarks>Builds a <see cref="FileName" /> from <see cref="Name" />, which wraps <see cref="Path.GetFileName(string)" />.</remarks>
    public FileName FileName
    {
        get
        {
            return new(Name);
        }
    }

    /// <summary>
    /// Gets the file extension portion of the path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetExtension(string)" />.</remarks>
    public string? Extension
    {
        get
        {
            return Path.GetExtension(FullName);
        }
    }

    /// <summary>
    /// Gets the parent directory path when one is available.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetDirectoryName(string)" />.</remarks>
    public DirectoryPath? ParentDirectory
    {
        get
        {
            var directoryName = Path.GetDirectoryName(FullName);
            if (directoryName is null)
            {
                return default;
            }

            return new DirectoryPath(directoryName);
        }
    }

    /// <summary>
    /// Gets the root directory information for the current path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetPathRoot(string)" />.</remarks>
    public string Root
    {
        get
        {
            return Path.GetPathRoot(FullName) ?? "";
        }
    }

    /// <summary>
    /// Gets a value indicating whether the file path has an extension.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.HasExtension(string)" />.</remarks>
    public bool HasExtension
    {
        get
        {
            return Path.HasExtension(FullName);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the path string contains a root.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.IsPathRooted(string)" />.</remarks>
    public bool IsPathRooted
    {
        get
        {
            return Path.IsPathRooted(FullName);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the path is fully qualified.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.IsPathFullyQualified(string)" />.</remarks>
    public bool IsFullyQualified
    {
        get
        {
            return Path.IsPathFullyQualified(FullName);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the file exists.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Exists(string)" />.</remarks>
    public bool Exists
    {
        get
        {
            return File.Exists(FullName);
        }
    }

    /// <summary>
    /// Gets the file length in bytes.
    /// </summary>
    /// <remarks>Reads <see cref="FileInfo.Length" /> from the <see cref="FileInfo" /> created by <see cref="ToFileInfo()" />.</remarks>
    public long Length
    {
        get
        {
            return ToFileInfo().Length;
        }
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
        get
        {
            return File.GetCreationTime(FullName);
        }

        set
        {
            File.SetCreationTime(FullName, value);
        }
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
        get
        {
            return File.GetCreationTimeUtc(FullName);
        }

        set
        {
            File.SetCreationTimeUtc(FullName, value);
        }
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
        get
        {
            return File.GetLastWriteTime(FullName);
        }

        set
        {
            File.SetLastWriteTime(FullName, value);
        }
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
        get
        {
            return File.GetLastWriteTimeUtc(FullName);
        }

        set
        {
            File.SetLastWriteTimeUtc(FullName, value);
        }
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
        get
        {
            return File.GetLastAccessTime(FullName);
        }

        set
        {
            File.SetLastAccessTime(FullName, value);
        }
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
        get
        {
            return File.GetLastAccessTimeUtc(FullName);
        }

        set
        {
            File.SetLastAccessTimeUtc(FullName, value);
        }
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
        get
        {
            return File.GetAttributes(FullName);
        }

        set
        {
            File.SetAttributes(FullName, value);
        }
    }
}
