// -----------------------------------------------------------------------
// <copyright file="DirectoryPath.Enumeration.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides enumeration and retrieval members for <see cref="DirectoryPath" />.
/// </summary>
public readonly partial record struct DirectoryPath
{
    /// <summary>
    /// Enumerates child directories.
    /// </summary>
    /// <returns>The child directory paths.</returns>
    public IEnumerable<DirectoryPath> EnumerateDirectories()
    {
        return EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Enumerates child directories matching a search pattern.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child directory paths.</returns>
    public IEnumerable<DirectoryPath> EnumerateDirectories(string searchPattern)
    {
        return EnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Enumerates child directories matching a search pattern and search option.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child directory paths.</returns>
    public IEnumerable<DirectoryPath> EnumerateDirectories(string searchPattern, SearchOption searchOption)
    {
        return Directory.EnumerateDirectories(FullName, searchPattern, searchOption).Select(static x => new DirectoryPath(x));
    }

    /// <summary>
    /// Enumerates child directories matching a search pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child directory paths.</returns>
    public IEnumerable<DirectoryPath> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return Directory.EnumerateDirectories(FullName, searchPattern, enumerationOptions).Select(static x => new DirectoryPath(x));
    }

    /// <summary>
    /// Gets child directories as an array.
    /// </summary>
    /// <returns>The child directory paths.</returns>
    public DirectoryPath[] GetDirectories()
    {
        return GetDirectories("*", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Gets child directories matching a search pattern as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child directory paths.</returns>
    public DirectoryPath[] GetDirectories(string searchPattern)
    {
        return GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Gets child directories matching a search pattern and search option as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child directory paths.</returns>
    public DirectoryPath[] GetDirectories(string searchPattern, SearchOption searchOption)
    {
        return Directory.GetDirectories(FullName, searchPattern, searchOption).Select(static x => new DirectoryPath(x)).ToArray();
    }

    /// <summary>
    /// Gets child directories matching a search pattern and enumeration options as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child directory paths.</returns>
    public DirectoryPath[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return Directory.GetDirectories(FullName, searchPattern, enumerationOptions).Select(static x => new DirectoryPath(x)).ToArray();
    }

    /// <summary>
    /// Enumerates child files.
    /// </summary>
    /// <returns>The child file paths.</returns>
    public IEnumerable<FilePath> EnumerateFiles()
    {
        return EnumerateFiles("*", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Enumerates child files matching a search pattern.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file paths.</returns>
    public IEnumerable<FilePath> EnumerateFiles(string searchPattern)
    {
        return EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Enumerates child files matching a search pattern and search option.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file paths.</returns>
    public IEnumerable<FilePath> EnumerateFiles(string searchPattern, SearchOption searchOption)
    {
        return Directory.EnumerateFiles(FullName, searchPattern, searchOption).Select(static x => new FilePath(x));
    }

    /// <summary>
    /// Enumerates child files matching a search pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file paths.</returns>
    public IEnumerable<FilePath> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return Directory.EnumerateFiles(FullName, searchPattern, enumerationOptions).Select(static x => new FilePath(x));
    }

    /// <summary>
    /// Gets child files as an array.
    /// </summary>
    /// <returns>The child file paths.</returns>
    public FilePath[] GetFiles()
    {
        return GetFiles("*", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Gets child files matching a search pattern as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file paths.</returns>
    public FilePath[] GetFiles(string searchPattern)
    {
        return GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Gets child files matching a search pattern and search option as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file paths.</returns>
    public FilePath[] GetFiles(string searchPattern, SearchOption searchOption)
    {
        return Directory.GetFiles(FullName, searchPattern, searchOption).Select(static x => new FilePath(x)).ToArray();
    }

    /// <summary>
    /// Gets child files matching a search pattern and enumeration options as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file paths.</returns>
    public FilePath[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return Directory.GetFiles(FullName, searchPattern, enumerationOptions).Select(static x => new FilePath(x)).ToArray();
    }

    /// <summary>
    /// Enumerates child file system entries.
    /// </summary>
    /// <returns>The child file system entry paths.</returns>
    public IEnumerable<string> EnumerateFileSystemEntries()
    {
        return EnumerateFileSystemEntries("*", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Enumerates child file system entries matching a search pattern.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file system entry paths.</returns>
    public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern)
    {
        return EnumerateFileSystemEntries(searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Enumerates child file system entries matching a search pattern and search option.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file system entry paths.</returns>
    public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern, SearchOption searchOption)
    {
        return Directory.EnumerateFileSystemEntries(FullName, searchPattern, searchOption);
    }

    /// <summary>
    /// Enumerates child file system entries matching a search pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file system entry paths.</returns>
    public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return Directory.EnumerateFileSystemEntries(FullName, searchPattern, enumerationOptions);
    }

    /// <summary>
    /// Gets child file system entries as an array.
    /// </summary>
    /// <returns>The child file system entry paths.</returns>
    public string[] GetFileSystemEntries()
    {
        return GetFileSystemEntries("*", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Gets child file system entries matching a search pattern as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file system entry paths.</returns>
    public string[] GetFileSystemEntries(string searchPattern)
    {
        return GetFileSystemEntries(searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    /// Gets child file system entries matching a search pattern and search option as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file system entry paths.</returns>
    public string[] GetFileSystemEntries(string searchPattern, SearchOption searchOption)
    {
        return Directory.GetFileSystemEntries(FullName, searchPattern, searchOption);
    }

    /// <summary>
    /// Gets child file system entries matching a search pattern and enumeration options as an array.
    /// </summary>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file system entry paths.</returns>
    public string[] GetFileSystemEntries(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return Directory.GetFileSystemEntries(FullName, searchPattern, enumerationOptions);
    }
}
