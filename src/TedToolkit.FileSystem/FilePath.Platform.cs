// -----------------------------------------------------------------------
// <copyright file="FilePath.Platform.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides platform-specific file operations for file paths.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Encrypts the file.
    /// </summary>
    public void Encrypt()
    {
        File.Encrypt(FullName);
    }

    /// <summary>
    /// Decrypts the file.
    /// </summary>
    public void Decrypt()
    {
        File.Decrypt(FullName);
    }

    /// <summary>
    /// Creates a symbolic link at the current file path.
    /// </summary>
    /// <param name="pathToTarget">The target path for the symbolic link.</param>
    /// <returns>The created symbolic link information.</returns>
    public FileSystemInfo CreateSymbolicLink(string pathToTarget)
    {
        return File.CreateSymbolicLink(FullName, pathToTarget);
    }

    /// <summary>
    /// Resolves the symbolic link target for the current file path.
    /// </summary>
    /// <param name="returnFinalTarget">A value indicating whether the final target should be resolved.</param>
    /// <returns>The resolved link target information.</returns>
    public FileSystemInfo? ResolveLinkTarget(bool returnFinalTarget)
    {
        return File.ResolveLinkTarget(FullName, returnFinalTarget);
    }

    /// <summary>
    /// Gets or sets the Unix file mode for the current file path.
    /// </summary>
    public UnixFileMode UnixFileMode
    {
        get
        {
            return File.GetUnixFileMode(FullName);
        }

        set
        {
            File.SetUnixFileMode(FullName, value);
        }
    }
}
