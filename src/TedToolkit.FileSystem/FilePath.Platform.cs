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
    /// <remarks>Wraps <see cref="File.Encrypt(string)" />.</remarks>
#if NET6_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Encrypt()
        {
            File.Encrypt(FullName);
        }

    /// <summary>
    /// Decrypts the file.
    /// </summary>
    /// <remarks>Wraps <see cref="File.Decrypt(string)" />.</remarks>
#if NET6_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Decrypt()
        {
            File.Decrypt(FullName);
        }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Creates a symbolic link at the current file path.
    /// </summary>
    /// <remarks>Wraps <see cref="File.CreateSymbolicLink(string,string)" />.</remarks>
    /// <param name="pathToTarget">The target path for the symbolic link.</param>
    /// <returns>The created symbolic link information.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileSystemInfo CreateSymbolicLink(string pathToTarget)
        {
            return File.CreateSymbolicLink(FullName, pathToTarget);
        }

    /// <summary>
    /// Resolves the symbolic link target for the current file path.
    /// </summary>
    /// <remarks>Wraps <see cref="File.ResolveLinkTarget(string,bool)" />.</remarks>
    /// <param name="returnFinalTarget">A value indicating whether the final target should be resolved.</param>
    /// <returns>The resolved link target information.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FileSystemInfo? ResolveLinkTarget(bool returnFinalTarget)
        {
            return File.ResolveLinkTarget(FullName, returnFinalTarget);
        }
#endif
#if NET7_0_OR_GREATER

    /// <summary>
    /// Gets or sets the Unix file mode for the current file path.
    /// </summary>
    /// <remarks>
    /// The getter wraps <see cref="File.GetUnixFileMode(string)" />.
    /// The setter wraps <see cref="File.SetUnixFileMode(string,System.IO.UnixFileMode)" />.
    /// </remarks>
    [System.Runtime.Versioning.UnsupportedOSPlatform("windows")]
    public UnixFileMode UnixFileMode
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return File.GetUnixFileMode(FullName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            File.SetUnixFileMode(FullName, value);
        }
    }
#endif
}