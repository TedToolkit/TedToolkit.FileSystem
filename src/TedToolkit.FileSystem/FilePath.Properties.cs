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
