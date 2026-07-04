// -----------------------------------------------------------------------
// <copyright file="FilePath.Directory.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides parent-directory helpers for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Creates the parent directory when it does not already exist.
    /// </summary>
    /// <returns>The created or existing parent directory path.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the file path does not contain a parent directory.</exception>
    public DirectoryPath CreateParentDirectory()
    {
        if (ParentDirectory is not { } directoryPath)
        {
            throw new InvalidOperationException("The file path does not contain a parent directory.");
        }

        return Directory.CreateDirectory(directoryPath.FullName);
    }

    /// <summary>
    /// Ensures that the parent directory exists.
    /// </summary>
    /// <returns>The current file path.</returns>
    public FilePath EnsureParentDirectoryExists()
    {
        _ = CreateParentDirectory();
        return this;
    }
}
