// -----------------------------------------------------------------------
// <copyright file="FilePath.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Represents a file path value.
/// </summary>
/// <param name="FullName">The full file path text.</param>
public readonly partial record struct FilePath(string FullName)
{
    /// <summary>
    /// Creates a file path value object from a file info instance.
    /// </summary>
    /// <remarks>Uses the <see cref="FileInfo.FullName" /> value from the supplied <see cref="FileInfo" /> instance.</remarks>
    /// <param name="fileInfo">The file info instance to convert.</param>
    /// <returns>A file path value object that uses the file full name.</returns>
    public static FilePath FromFileInfo(FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo);
        return new(fileInfo.FullName);
    }

    /// <summary>
    /// Converts a file info instance into a file path value object.
    /// </summary>
    /// <remarks>Delegates to <see cref="FromFileInfo(FileInfo)" />, which uses the <see cref="FileInfo.FullName" /> value.</remarks>
    /// <param name="fileInfo">The file info instance to convert.</param>
    public static implicit operator FilePath(FileInfo fileInfo)
    {
        return FromFileInfo(fileInfo);
    }
}
