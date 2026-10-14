// -----------------------------------------------------------------------
// <copyright file="FileSystemGuardExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides explicit precondition checks for file paths, directory paths, and their result collections.
/// </summary>
/// <remarks>
/// Messages follow TedToolkit.Localizations.LocalizationSettings.Culture.
/// Path checks validate the stored text, including default value objects, without changing the path value.
/// File system checks observe the current state and do not reserve a path for a later operation.
/// </remarks>
public static class FileSystemGuardExtensions
{
    /// <summary>
    /// Throws if the file path or its stored text is null.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    public static void ThrowIfNull(
        this FilePath filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        CheckNotNullText(filePath.FullName, paramName);
    }

    /// <summary>
    /// Throws if the file path is null or its stored text is null, empty, or whitespace.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only, using string.IsNullOrWhiteSpace; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    public static void ThrowIfNullOrWhiteSpace(
        this FilePath filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        GetRequiredPath(filePath, paramName);
    }

    /// <summary>
    /// Throws if the file path is invalid or does not identify an existing file.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses FilePath.Exists (File.Exists); failure diagnostics use FilePath.GetFullPath (Path.GetFullPath).</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="FileNotFoundException">File.Exists reports that the path is not an existing file.</exception>
    public static void ThrowIfNotExists(
        this FilePath filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        var path = GetRequiredPath(filePath, paramName);
        if (path.Exists)
        {
            return;
        }

        var absolutePath = path.GetFullPath().FullName;
        throw new FileNotFoundException(
            Localization.FileSystemGuards.FileNotFound(paramName ?? "", absolutePath), absolutePath);
    }

    /// <summary>
    /// Throws if the file path is invalid or an existing file or directory occupies that path.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses FilePath.Exists and DirectoryPath.Exists (File.Exists and Directory.Exists) on every supported target framework. This is not a permissions check.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="IOException">An existing file or directory occupies the path.</exception>
    public static void ThrowIfExists(
        this FilePath filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        var path = GetRequiredPath(filePath, paramName);
        if (!path.Exists && !new DirectoryPath(path.FullName).Exists)
        {
            return;
        }

        throw new IOException(Localization.FileSystemGuards.PathAlreadyExists(
            paramName ?? "", path.GetFullPath().FullName));
    }

    /// <summary>
    /// Throws if the file path or its stored text is null.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    public static void ThrowIfNull(
        [NotNull] this FilePath? filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        CheckNotNullText(GetPathText(filePath, paramName), paramName);
    }

    /// <summary>
    /// Throws if the file path is null or its stored text is null, empty, or whitespace.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only, using string.IsNullOrWhiteSpace; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    public static void ThrowIfNullOrWhiteSpace(
        [NotNull] this FilePath? filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        GetRequiredPath(filePath, paramName);
    }

    /// <summary>
    /// Throws if the file path is invalid or does not identify an existing file.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses FilePath.Exists (File.Exists); failure diagnostics use FilePath.GetFullPath (Path.GetFullPath).</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="FileNotFoundException">File.Exists reports that the path is not an existing file.</exception>
    public static void ThrowIfNotExists(
        [NotNull] this FilePath? filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        var path = GetRequiredPath(filePath, paramName);
        if (path.Exists)
        {
            return;
        }

        var absolutePath = path.GetFullPath().FullName;
        throw new FileNotFoundException(
            Localization.FileSystemGuards.FileNotFound(paramName ?? "", absolutePath), absolutePath);
    }

    /// <summary>
    /// Throws if the file path is invalid or an existing file or directory occupies that path.
    /// </summary>
    /// <param name="filePath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses FilePath.Exists and DirectoryPath.Exists (File.Exists and Directory.Exists) on every supported target framework. This is not a permissions check.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="IOException">An existing file or directory occupies the path.</exception>
    public static void ThrowIfExists(
        [NotNull] this FilePath? filePath,
        [CallerArgumentExpression(nameof(filePath))] string? paramName = null)
    {
        var path = GetRequiredPath(filePath, paramName);
        if (!path.Exists && !new DirectoryPath(path.FullName).Exists)
        {
            return;
        }

        throw new IOException(Localization.FileSystemGuards.PathAlreadyExists(
            paramName ?? "", path.GetFullPath().FullName));
    }

    /// <summary>
    /// Throws if the directory path or its stored text is null.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    public static void ThrowIfNull(
        this DirectoryPath directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        CheckNotNullText(directoryPath.FullName, paramName);
    }

    /// <summary>
    /// Throws if the directory path is null or its stored text is null, empty, or whitespace.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only, using string.IsNullOrWhiteSpace; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    public static void ThrowIfNullOrWhiteSpace(
        this DirectoryPath directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        GetRequiredPath(directoryPath, paramName);
    }

    /// <summary>
    /// Throws if the directory path is invalid or does not identify an existing directory.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses DirectoryPath.Exists (Directory.Exists); failure diagnostics use DirectoryPath.GetFullPath (Path.GetFullPath).</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="DirectoryNotFoundException">Directory.Exists reports that the path is not an existing directory.</exception>
    public static void ThrowIfNotExists(
        this DirectoryPath directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        GetExistingDirectory(directoryPath, paramName);
    }

    /// <summary>
    /// Throws if the directory path is invalid or an existing file or directory occupies that path.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses FilePath.Exists and DirectoryPath.Exists (File.Exists and Directory.Exists) on every supported target framework. This is not a permissions check.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="IOException">An existing file or directory occupies the path.</exception>
    public static void ThrowIfExists(
        this DirectoryPath directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        var path = GetRequiredPath(directoryPath, paramName);
        if (!new FilePath(path.FullName).Exists && !path.Exists)
        {
            return;
        }

        throw new IOException(Localization.FileSystemGuards.PathAlreadyExists(
            paramName ?? "", path.GetFullPath().FullName));
    }

    /// <summary>
    /// Throws if the directory path or its stored text is null.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    public static void ThrowIfNull(
        [NotNull] this DirectoryPath? directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        CheckNotNullText(GetPathText(directoryPath, paramName), paramName);
    }

    /// <summary>
    /// Throws if the directory path is null or its stored text is null, empty, or whitespace.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Checks path text only, using string.IsNullOrWhiteSpace; no file system query is performed.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    public static void ThrowIfNullOrWhiteSpace(
        [NotNull] this DirectoryPath? directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        GetRequiredPath(directoryPath, paramName);
    }

    /// <summary>
    /// Throws if the directory path is invalid or does not identify an existing directory.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses DirectoryPath.Exists (Directory.Exists); failure diagnostics use DirectoryPath.GetFullPath (Path.GetFullPath).</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="DirectoryNotFoundException">Directory.Exists reports that the path is not an existing directory.</exception>
    public static void ThrowIfNotExists(
        [NotNull] this DirectoryPath? directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        GetExistingDirectory(directoryPath, paramName);
    }

    /// <summary>
    /// Throws if the directory path is invalid or an existing file or directory occupies that path.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses FilePath.Exists and DirectoryPath.Exists (File.Exists and Directory.Exists) on every supported target framework. This is not a permissions check.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="IOException">An existing file or directory occupies the path.</exception>
    public static void ThrowIfExists(
        [NotNull] this DirectoryPath? directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        var path = GetRequiredPath(directoryPath, paramName);
        if (!new FilePath(path.FullName).Exists && !path.Exists)
        {
            return;
        }

        throw new IOException(Localization.FileSystemGuards.PathAlreadyExists(
            paramName ?? "", path.GetFullPath().FullName));
    }

    /// <summary>
    /// Throws if the directory is invalid, missing, or contains no matching files.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="searchPattern">The native BCL wildcard pattern, such as *.json.</param>
    /// <param name="searchOption">The search scope; the default checks only the current directory.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <param name="searchPatternParamName">The automatically captured search-pattern expression.</param>
    /// <remarks>Uses DirectoryPath.EnumerateFiles (Directory.EnumerateFiles). Enumeration stops at the first match and no result array is allocated. Directory validation precedes pattern validation. A zero-byte file is still a file; an empty child directory is still a directory.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text or search pattern is blank, or the BCL rejects the pattern.</exception>
    /// <exception cref="DirectoryNotFoundException">The directory does not exist.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The search option is not supported.</exception>
    /// <exception cref="InvalidOperationException">No matching files exist in the selected scope.</exception>
    /// <exception cref="UnauthorizedAccessException">The directory cannot be enumerated with the current permissions.</exception>
    /// <exception cref="IOException">An I/O error occurs during enumeration.</exception>
    public static void ThrowIfContainsNoFiles(
        this DirectoryPath directoryPath,
        string? searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null,
        [CallerArgumentExpression(nameof(searchPattern))] string? searchPatternParamName = null)
    {
        var path = GetExistingDirectory(directoryPath, paramName);
        CheckPattern(searchPattern, searchPatternParamName);
        if (path.EnumerateFiles(searchPattern, searchOption).Any())
        {
            return;
        }

        throw new InvalidOperationException(Localization.FileSystemGuards.NoMatchingFiles(
            paramName ?? "", path.GetFullPath().FullName, searchPattern, searchOption.ToString()));
    }

    /// <summary>
    /// Throws if the directory is invalid, missing, or contains no matching subdirectories.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="searchPattern">The native BCL wildcard pattern, such as *.json.</param>
    /// <param name="searchOption">The search scope; the default checks only the current directory.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <param name="searchPatternParamName">The automatically captured search-pattern expression.</param>
    /// <remarks>Uses DirectoryPath.EnumerateDirectories (Directory.EnumerateDirectories). Enumeration stops at the first match and no result array is allocated. Directory validation precedes pattern validation. A zero-byte file is still a file; an empty child directory is still a directory.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text or search pattern is blank, or the BCL rejects the pattern.</exception>
    /// <exception cref="DirectoryNotFoundException">The directory does not exist.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The search option is not supported.</exception>
    /// <exception cref="InvalidOperationException">No matching subdirectories exist in the selected scope.</exception>
    /// <exception cref="UnauthorizedAccessException">The directory cannot be enumerated with the current permissions.</exception>
    /// <exception cref="IOException">An I/O error occurs during enumeration.</exception>
    public static void ThrowIfContainsNoFolders(
        this DirectoryPath directoryPath,
        string? searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null,
        [CallerArgumentExpression(nameof(searchPattern))] string? searchPatternParamName = null)
    {
        var path = GetExistingDirectory(directoryPath, paramName);
        CheckPattern(searchPattern, searchPatternParamName);
        if (path.EnumerateDirectories(searchPattern, searchOption).Any())
        {
            return;
        }

        throw new InvalidOperationException(Localization.FileSystemGuards.NoMatchingFolders(
            paramName ?? "", path.GetFullPath().FullName, searchPattern, searchOption.ToString()));
    }

    /// <summary>
    /// Throws if the directory is invalid, missing, or contains neither files nor subdirectories.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses DirectoryPath.EnumerateFileSystemEntries (Directory.EnumerateFileSystemEntries) for the current directory only. An empty child directory counts as an entry.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="DirectoryNotFoundException">The directory does not exist.</exception>
    /// <exception cref="InvalidOperationException">The directory has no entries.</exception>
    /// <exception cref="UnauthorizedAccessException">The directory cannot be enumerated with the current permissions.</exception>
    /// <exception cref="IOException">An I/O error occurs during enumeration.</exception>
    public static void ThrowIfContainsNoEntries(
        this DirectoryPath directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        var path = GetExistingDirectory(directoryPath, paramName);
        if (path.EnumerateFileSystemEntries().Any())
        {
            return;
        }

        throw new InvalidOperationException(Localization.FileSystemGuards.NoEntries(
            paramName ?? "", path.GetFullPath().FullName));
    }

    /// <summary>
    /// Throws if the directory is invalid, missing, or contains no matching files.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="searchPattern">The native BCL wildcard pattern, such as *.json.</param>
    /// <param name="searchOption">The search scope; the default checks only the current directory.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <param name="searchPatternParamName">The automatically captured search-pattern expression.</param>
    /// <remarks>Uses DirectoryPath.EnumerateFiles (Directory.EnumerateFiles). Enumeration stops at the first match and no result array is allocated. Directory validation precedes pattern validation. A zero-byte file is still a file; an empty child directory is still a directory.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text or search pattern is blank, or the BCL rejects the pattern.</exception>
    /// <exception cref="DirectoryNotFoundException">The directory does not exist.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The search option is not supported.</exception>
    /// <exception cref="InvalidOperationException">No matching files exist in the selected scope.</exception>
    /// <exception cref="UnauthorizedAccessException">The directory cannot be enumerated with the current permissions.</exception>
    /// <exception cref="IOException">An I/O error occurs during enumeration.</exception>
    public static void ThrowIfContainsNoFiles(
        [NotNull] this DirectoryPath? directoryPath,
        string? searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null,
        [CallerArgumentExpression(nameof(searchPattern))] string? searchPatternParamName = null)
    {
        var path = GetExistingDirectory(directoryPath, paramName);
        CheckPattern(searchPattern, searchPatternParamName);
        if (path.EnumerateFiles(searchPattern, searchOption).Any())
        {
            return;
        }

        throw new InvalidOperationException(Localization.FileSystemGuards.NoMatchingFiles(
            paramName ?? "", path.GetFullPath().FullName, searchPattern, searchOption.ToString()));
    }

    /// <summary>
    /// Throws if the directory is invalid, missing, or contains no matching subdirectories.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="searchPattern">The native BCL wildcard pattern, such as *.json.</param>
    /// <param name="searchOption">The search scope; the default checks only the current directory.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <param name="searchPatternParamName">The automatically captured search-pattern expression.</param>
    /// <remarks>Uses DirectoryPath.EnumerateDirectories (Directory.EnumerateDirectories). Enumeration stops at the first match and no result array is allocated. Directory validation precedes pattern validation. A zero-byte file is still a file; an empty child directory is still a directory.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text or search pattern is blank, or the BCL rejects the pattern.</exception>
    /// <exception cref="DirectoryNotFoundException">The directory does not exist.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The search option is not supported.</exception>
    /// <exception cref="InvalidOperationException">No matching subdirectories exist in the selected scope.</exception>
    /// <exception cref="UnauthorizedAccessException">The directory cannot be enumerated with the current permissions.</exception>
    /// <exception cref="IOException">An I/O error occurs during enumeration.</exception>
    public static void ThrowIfContainsNoFolders(
        [NotNull] this DirectoryPath? directoryPath,
        string? searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null,
        [CallerArgumentExpression(nameof(searchPattern))] string? searchPatternParamName = null)
    {
        var path = GetExistingDirectory(directoryPath, paramName);
        CheckPattern(searchPattern, searchPatternParamName);
        if (path.EnumerateDirectories(searchPattern, searchOption).Any())
        {
            return;
        }

        throw new InvalidOperationException(Localization.FileSystemGuards.NoMatchingFolders(
            paramName ?? "", path.GetFullPath().FullName, searchPattern, searchOption.ToString()));
    }

    /// <summary>
    /// Throws if the directory is invalid, missing, or contains neither files nor subdirectories.
    /// </summary>
    /// <param name="directoryPath">The path value to check.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Uses DirectoryPath.EnumerateFileSystemEntries (Directory.EnumerateFileSystemEntries) for the current directory only. An empty child directory counts as an entry.</remarks>
    /// <exception cref="ArgumentNullException">The nullable path or its stored path text is null.</exception>
    /// <exception cref="ArgumentException">The stored path text is empty or whitespace.</exception>
    /// <exception cref="DirectoryNotFoundException">The directory does not exist.</exception>
    /// <exception cref="InvalidOperationException">The directory has no entries.</exception>
    /// <exception cref="UnauthorizedAccessException">The directory cannot be enumerated with the current permissions.</exception>
    /// <exception cref="IOException">An I/O error occurs during enumeration.</exception>
    public static void ThrowIfContainsNoEntries(
        [NotNull] this DirectoryPath? directoryPath,
        [CallerArgumentExpression(nameof(directoryPath))] string? paramName = null)
    {
        var path = GetExistingDirectory(directoryPath, paramName);
        if (path.EnumerateFileSystemEntries().Any())
        {
            return;
        }

        throw new InvalidOperationException(Localization.FileSystemGuards.NoEntries(
            paramName ?? "", path.GetFullPath().FullName));
    }

    /// <summary>
    /// Throws if the files collection is null or has no items.
    /// </summary>
    /// <param name="files">The filesystem result collection with an available item count.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Reads IReadOnlyCollection.Count only; does not enumerate items or check their paths.</remarks>
    /// <exception cref="ArgumentNullException">The collection is null.</exception>
    /// <exception cref="ArgumentException">The collection has no items.</exception>
    public static void ThrowIfNullOrHasNoItems(
        [NotNull] this IReadOnlyCollection<FilePath>? files,
        [CallerArgumentExpression(nameof(files))] string? paramName = null)
    {
        CheckCollection(files, paramName);
    }

    /// <summary>
    /// Throws if the directories collection is null or has no items.
    /// </summary>
    /// <param name="directories">The filesystem result collection with an available item count.</param>
    /// <param name="paramName">The automatically captured caller expression, or an explicit diagnostic name.</param>
    /// <remarks>Reads IReadOnlyCollection.Count only; does not enumerate items or check their paths.</remarks>
    /// <exception cref="ArgumentNullException">The collection is null.</exception>
    /// <exception cref="ArgumentException">The collection has no items.</exception>
    public static void ThrowIfNullOrHasNoItems(
        [NotNull] this IReadOnlyCollection<DirectoryPath>? directories,
        [CallerArgumentExpression(nameof(directories))] string? paramName = null)
    {
        CheckCollection(directories, paramName);
    }

    private static string GetPathText([NotNull] FilePath? filePath, string? paramName)
    {
        if (!filePath.HasValue)
        {
            ThrowNull(paramName);
        }

        var text = filePath.Value.FullName;
        CheckNotNullText(text, paramName);
        return text;
    }

    private static string GetPathText([NotNull] DirectoryPath? directoryPath, string? paramName)
    {
        if (!directoryPath.HasValue)
        {
            ThrowNull(paramName);
        }

        var text = directoryPath.Value.FullName;
        CheckNotNullText(text, paramName);
        return text;
    }

    private static FilePath GetRequiredPath([NotNull] FilePath? filePath, string? paramName)
    {
        var text = GetPathText(filePath, paramName);
        CheckNonBlankText(text, paramName);
        return filePath.Value;
    }

    private static DirectoryPath GetRequiredPath([NotNull] DirectoryPath? directoryPath, string? paramName)
    {
        var text = GetPathText(directoryPath, paramName);
        CheckNonBlankText(text, paramName);
        return directoryPath.Value;
    }

    private static DirectoryPath GetExistingDirectory([NotNull] DirectoryPath? directoryPath, string? paramName)
    {
        var path = GetRequiredPath(directoryPath, paramName);
        if (path.Exists)
        {
            return path;
        }

        throw new DirectoryNotFoundException(Localization.FileSystemGuards.FolderNotFound(
            paramName ?? "", path.GetFullPath().FullName));
    }

    private static void CheckNotNullText([NotNull] string? text, string? paramName)
    {
        if (text is not null)
        {
            return;
        }

        ThrowNull(paramName);
    }

    private static void CheckNonBlankText([NotNull] string? text, string? paramName)
    {
        CheckNotNullText(text, paramName);
        if (!string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        throw new ArgumentException(Localization.FileSystemGuards.BlankPath(paramName ?? ""), paramName);
    }

    private static void CheckPattern([NotNull] string? searchPattern, string? paramName)
    {
        CheckNotNullText(searchPattern, paramName);
        if (!string.IsNullOrWhiteSpace(searchPattern))
        {
            return;
        }

        throw new ArgumentException(Localization.FileSystemGuards.BlankPattern(paramName ?? ""), paramName);
    }

    private static void CheckCollection<T>([NotNull] IReadOnlyCollection<T>? items, string? paramName)
    {
        if (items is null)
        {
            ThrowNull(paramName);
        }

        if (items.Count != 0)
        {
            return;
        }

        throw new ArgumentException(Localization.FileSystemGuards.NoItems(paramName ?? ""), paramName);
    }

    [DoesNotReturn]
    private static void ThrowNull(string? paramName)
    {
        throw new ArgumentNullException(paramName, Localization.FileSystemGuards.NullReference(paramName ?? ""));
    }
}