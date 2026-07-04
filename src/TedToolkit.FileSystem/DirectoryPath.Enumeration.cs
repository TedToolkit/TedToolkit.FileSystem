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
    /// <remarks>Wraps <see cref="Directory.EnumerateDirectories(string)" />.</remarks>
    /// <returns>The child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<DirectoryPath> EnumerateDirectories()
        {
            return Directory.EnumerateDirectories(FullName).Select(static x => new DirectoryPath(x));
        }

    /// <summary>
    /// Enumerates child directories matching a search pattern.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateDirectories(string,string)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<DirectoryPath> EnumerateDirectories(string searchPattern)
        {
            return Directory.EnumerateDirectories(FullName, searchPattern).Select(static x => new DirectoryPath(x));
        }

    /// <summary>
    /// Enumerates child directories matching a search pattern and search option.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateDirectories(string,string,System.IO.SearchOption)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<DirectoryPath> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(FullName, searchPattern, searchOption).Select(static x => new DirectoryPath(x));
        }

#if NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Enumerates child directories matching a search pattern and enumeration options.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateDirectories(string,string,System.IO.EnumerationOptions)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<DirectoryPath> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.EnumerateDirectories(FullName, searchPattern, enumerationOptions).Select(static x => new DirectoryPath(x));
        }
#endif

    /// <summary>
    /// Gets child directories as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetDirectories(string)" />.</remarks>
    /// <returns>The child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath[] GetDirectories()
        {
            return Directory.GetDirectories(FullName).Select(static x => new DirectoryPath(x)).ToArray();
        }

    /// <summary>
    /// Gets child directories matching a search pattern as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetDirectories(string,string)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath[] GetDirectories(string searchPattern)
        {
            return Directory.GetDirectories(FullName, searchPattern).Select(static x => new DirectoryPath(x)).ToArray();
        }

    /// <summary>
    /// Gets child directories matching a search pattern and search option as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetDirectories(string,string,System.IO.SearchOption)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(FullName, searchPattern, searchOption).Select(static x => new DirectoryPath(x)).ToArray();
        }

#if NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Gets child directories matching a search pattern and enumeration options as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetDirectories(string,string,System.IO.EnumerationOptions)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child directory paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DirectoryPath[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.GetDirectories(FullName, searchPattern, enumerationOptions).Select(static x => new DirectoryPath(x)).ToArray();
        }
#endif

    /// <summary>
    /// Enumerates child files.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFiles(string)" />.</remarks>
    /// <returns>The child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FilePath> EnumerateFiles()
        {
            return Directory.EnumerateFiles(FullName).Select(static x => new FilePath(x));
        }

    /// <summary>
    /// Enumerates child files matching a search pattern.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFiles(string,string)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FilePath> EnumerateFiles(string searchPattern)
        {
            return Directory.EnumerateFiles(FullName, searchPattern).Select(static x => new FilePath(x));
        }

    /// <summary>
    /// Enumerates child files matching a search pattern and search option.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFiles(string,string,System.IO.SearchOption)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FilePath> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(FullName, searchPattern, searchOption).Select(static x => new FilePath(x));
        }

#if NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Enumerates child files matching a search pattern and enumeration options.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFiles(string,string,System.IO.EnumerationOptions)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<FilePath> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.EnumerateFiles(FullName, searchPattern, enumerationOptions).Select(static x => new FilePath(x));
        }
#endif

    /// <summary>
    /// Gets child files as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFiles(string)" />.</remarks>
    /// <returns>The child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath[] GetFiles()
        {
            return Directory.GetFiles(FullName).Select(static x => new FilePath(x)).ToArray();
        }

    /// <summary>
    /// Gets child files matching a search pattern as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFiles(string,string)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath[] GetFiles(string searchPattern)
        {
            return Directory.GetFiles(FullName, searchPattern).Select(static x => new FilePath(x)).ToArray();
        }

    /// <summary>
    /// Gets child files matching a search pattern and search option as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFiles(string,string,System.IO.SearchOption)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(FullName, searchPattern, searchOption).Select(static x => new FilePath(x)).ToArray();
        }

#if NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Gets child files matching a search pattern and enumeration options as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFiles(string,string,System.IO.EnumerationOptions)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.GetFiles(FullName, searchPattern, enumerationOptions).Select(static x => new FilePath(x)).ToArray();
        }
#endif

    /// <summary>
    /// Enumerates child file system entries.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFileSystemEntries(string)" />.</remarks>
    /// <returns>The child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<string> EnumerateFileSystemEntries()
        {
            return Directory.EnumerateFileSystemEntries(FullName);
        }

    /// <summary>
    /// Enumerates child file system entries matching a search pattern.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFileSystemEntries(string,string)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(FullName, searchPattern);
        }

    /// <summary>
    /// Enumerates child file system entries matching a search pattern and search option.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFileSystemEntries(string,string,System.IO.SearchOption)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(FullName, searchPattern, searchOption);
        }

#if NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Enumerates child file system entries matching a search pattern and enumeration options.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.EnumerateFileSystemEntries(string,string,System.IO.EnumerationOptions)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<string> EnumerateFileSystemEntries(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.EnumerateFileSystemEntries(FullName, searchPattern, enumerationOptions);
        }
#endif

    /// <summary>
    /// Gets child file system entries as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFileSystemEntries(string)" />.</remarks>
    /// <returns>The child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string[] GetFileSystemEntries()
        {
            return Directory.GetFileSystemEntries(FullName);
        }

    /// <summary>
    /// Gets child file system entries matching a search pattern as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFileSystemEntries(string,string)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <returns>The matching child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string[] GetFileSystemEntries(string searchPattern)
        {
            return Directory.GetFileSystemEntries(FullName, searchPattern);
        }

    /// <summary>
    /// Gets child file system entries matching a search pattern and search option as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFileSystemEntries(string,string,System.IO.SearchOption)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The matching child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string[] GetFileSystemEntries(string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFileSystemEntries(FullName, searchPattern, searchOption);
        }

#if NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Gets child file system entries matching a search pattern and enumeration options as an array.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetFileSystemEntries(string,string,System.IO.EnumerationOptions)" />.</remarks>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="enumerationOptions">The enumeration options.</param>
    /// <returns>The matching child file system entry paths.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string[] GetFileSystemEntries(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.GetFileSystemEntries(FullName, searchPattern, enumerationOptions);
        }
#endif
}