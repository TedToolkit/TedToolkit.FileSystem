// -----------------------------------------------------------------------
// <copyright file="FileSystemExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem
{
/// <summary>
/// Provides extension methods for file system value objects.
/// </summary>
public static class FileSystemExtensions
{
    /// <summary>
    /// Wraps a string value as a file name value object.
    /// </summary>
    /// <remarks>Constructs <see cref="FileName" /> directly and does not wrap an additional BCL API.</remarks>
    /// <param name="value">The file name text.</param>
    /// <returns>A file name value object.</returns>
    public static FileName AsFileName(this string value)
    {
        return new(value);
    }

    /// <summary>
    /// Converts a directory info instance to a directory path value object.
    /// </summary>
    /// <remarks>Delegates to the implicit <see cref="DirectoryPath" /> conversion, which uses the `DirectoryInfo.FullName` value.</remarks>
    /// <param name="directoryInfo">The directory info instance to convert.</param>
    /// <returns>A directory path value object.</returns>
    public static DirectoryPath ToPath(this DirectoryInfo directoryInfo)
    {
        return directoryInfo;
    }

    /// <summary>
    /// Converts a file info instance to a file path value object.
    /// </summary>
    /// <remarks>Delegates to the implicit <see cref="FilePath" /> conversion, which uses the `FileInfo.FullName` value.</remarks>
    /// <param name="fileInfo">The file info instance to convert.</param>
    /// <returns>A file path value object.</returns>
    public static FilePath ToPath(this FileInfo fileInfo)
    {
        return fileInfo;
    }
}

/// <summary>
/// Provides compatibility helpers for target frameworks with missing runtime helpers.
/// </summary>
internal static class Compatibility
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException" /> when the supplied reference is <see langword="null" />.
    /// </summary>
    /// <typeparam name="T">The reference type to validate.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The validated parameter name.</param>
    public static void ThrowIfNull<T>(T value, string paramName)
        where T : class
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value, paramName);
#else
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
#endif
    }

    /// <summary>
    /// Removes a trailing directory separator when the path is longer than its root.
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized path.</returns>
    public static string TrimEndingDirectorySeparator(string path)
    {
        var root = Path.GetPathRoot(path) ?? "";

        while (path.Length > root.Length && EndsInDirectorySeparator(path))
        {
            path = path.Substring(0, path.Length - 1);
        }

        return path;
    }

    /// <summary>
    /// Determines whether the path ends in a directory separator.
    /// </summary>
    /// <param name="path">The path to inspect.</param>
    /// <returns><see langword="true" /> when the path ends in a directory separator; otherwise, <see langword="false" />.</returns>
    public static bool EndsInDirectorySeparator(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        var lastChar = path[path.Length - 1];
        return lastChar == Path.DirectorySeparatorChar || lastChar == Path.AltDirectorySeparatorChar;
    }

    /// <summary>
    /// Gets an absolute path using the supplied base directory.
    /// </summary>
    /// <param name="path">The path to resolve.</param>
    /// <param name="basePath">The base path to resolve against.</param>
    /// <returns>The absolute path.</returns>
    public static string GetFullPath(string path, string basePath)
    {
        return Path.GetFullPath(Path.Combine(basePath, path));
    }

    /// <summary>
    /// Gets the relative path from one path to another.
    /// </summary>
    /// <param name="relativeTo">The base path.</param>
    /// <param name="path">The target path.</param>
    /// <returns>The relative path.</returns>
    public static string GetRelativePath(string relativeTo, string path)
    {
        var baseUri = new Uri(AppendDirectorySeparator(Path.GetFullPath(relativeTo)));
        var targetUri = new Uri(Path.GetFullPath(path));

        return Uri.UnescapeDataString(baseUri.MakeRelativeUri(targetUri).ToString())
            .Replace('/', Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// Determines whether the supplied path is fully qualified.
    /// </summary>
    /// <param name="path">The path to inspect.</param>
    /// <returns><see langword="true" /> when the path is fully qualified; otherwise, <see langword="false" />.</returns>
    public static bool IsPathFullyQualified(string path)
    {
#if NET6_0_OR_GREATER
        return Path.IsPathFullyQualified(path);
#else
        if (!Path.IsPathRooted(path))
        {
            return false;
        }

        var root = Path.GetPathRoot(path);
        return !string.IsNullOrEmpty(root) && path.Length > root.Length;
#endif
    }

    /// <summary>
    /// Moves a file and overwrites the destination when requested.
    /// </summary>
    /// <param name="sourcePath">The source file path.</param>
    /// <param name="destinationPath">The destination file path.</param>
    /// <param name="overwrite">Whether the destination should be overwritten.</param>
    public static void MoveFile(string sourcePath, string destinationPath, bool overwrite)
    {
#if NET6_0_OR_GREATER
        File.Move(sourcePath, destinationPath, overwrite);
#else
        if (overwrite && File.Exists(destinationPath))
        {
            File.Delete(destinationPath);
        }

        File.Move(sourcePath, destinationPath);
#endif
    }

    private static string AppendDirectorySeparator(string path)
    {
        return EndsInDirectorySeparator(path) ? path : path + Path.DirectorySeparatorChar;
    }
}
}

#if NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
namespace System.Runtime.CompilerServices
{
/// <summary>
/// Provides init-only support for target frameworks that do not define the runtime type.
/// </summary>
internal static class IsExternalInit;
}
#endif
