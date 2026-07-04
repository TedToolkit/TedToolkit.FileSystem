// -----------------------------------------------------------------------
// <copyright file="FilePath.Operations.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides file lifecycle operations for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Copies the file to another file path.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Copy(string,string,bool)" />.</remarks>
    /// <param name="destination">The destination file path.</param>
    /// <param name="overwrite">A value indicating whether an existing destination should be overwritten.</param>
    /// <returns>The destination file path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath CopyTo(FilePath destination, bool overwrite = false)
    {
        File.Copy(FullName, destination.FullName, overwrite);
        return destination;
    }

    /// <summary>
    /// Moves the file to another file path.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Move(string,string)" />.</remarks>
    /// <param name="destination">The destination file path.</param>
    /// <returns>The destination file path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath MoveTo(FilePath destination)
    {
        File.Move(FullName, destination.FullName);
        return destination;
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Moves the file to another file path and optionally overwrites the destination.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Move(string,string,bool)" />.</remarks>
    /// <param name="destination">The destination file path.</param>
    /// <param name="overwrite">A value indicating whether an existing destination should be overwritten.</param>
    /// <returns>The destination file path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath MoveTo(FilePath destination, bool overwrite)
    {
        File.Move(FullName, destination.FullName, overwrite);
        return destination;
    }
#endif

    /// <summary>
    /// Deletes the file.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Delete(string)" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Delete()
        {
            File.Delete(FullName);
        }

    /// <summary>
    /// Replaces the destination file with the current file and optionally creates a backup.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Replace(string,string,string?,bool)" />.</remarks>
    /// <param name="destination">The destination file path to replace.</param>
    /// <param name="backup">The optional backup file path.</param>
    /// <param name="ignoreMetadataErrors">A value indicating whether metadata errors should be ignored.</param>
    /// <returns>The destination file path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilePath Replace(FilePath destination, FilePath? backup = null, bool ignoreMetadataErrors = false)
    {
        File.Replace(FullName, destination.FullName, backup?.FullName, ignoreMetadataErrors);
        return destination;
    }
}