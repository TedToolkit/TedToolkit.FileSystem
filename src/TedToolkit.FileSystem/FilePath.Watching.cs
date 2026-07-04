// -----------------------------------------------------------------------
// <copyright file="FilePath.Watching.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides file system watching helpers for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Creates a file system watcher scoped to the current file path.
    /// </summary>
    /// <remarks>
    /// Creates a new <see cref="FileSystemWatcher" /> via <see cref="FileSystemWatcher.FileSystemWatcher(string,string)" />
    /// and enables <see cref="FileSystemWatcher.EnableRaisingEvents" />.
    /// </remarks>
    /// <returns>A file system watcher for the current file path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileSystemWatcher Watch()
    {
        return new(Path.GetDirectoryName(FullName)!, Name) { EnableRaisingEvents = true, };
    }
}