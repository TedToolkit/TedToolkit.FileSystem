// -----------------------------------------------------------------------
// <copyright file="DirectoryPath.Static.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides static directory-scoped members for <see cref="DirectoryPath" />.
/// </summary>
public readonly partial record struct DirectoryPath
{
    /// <summary>
    /// Gets or sets the current working directory.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="Directory.GetCurrentDirectory()" />.
    /// The setter wraps <see cref="Directory.SetCurrentDirectory(string)" />.
    /// </remarks>
    public static DirectoryPath CurrentDirectory
    {
        get => new(Directory.GetCurrentDirectory());
        set => Directory.SetCurrentDirectory(value.FullName);
    }

    /// <summary>
    /// Gets the logical drives available on the current machine.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetLogicalDrives()" />.</remarks>
    public static DirectoryPath[] LogicalDrives => Directory.GetLogicalDrives().Select(static x => new DirectoryPath(x)).ToArray();

    /// <summary>
    /// Creates a temporary subdirectory using the specified prefix.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.CreateTempSubdirectory(string)" />.</remarks>
    /// <param name="prefix">The prefix for the temporary directory name.</param>
    /// <returns>The created temporary directory path.</returns>
    public static DirectoryPath CreateTempSubdirectory(string prefix)
    {
        return new(Directory.CreateTempSubdirectory(prefix).FullName);
    }
}
