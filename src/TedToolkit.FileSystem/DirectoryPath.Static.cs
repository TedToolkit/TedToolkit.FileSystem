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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Directory.GetCurrentDirectory());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Directory.SetCurrentDirectory(value.FullName);
    }

    /// <summary>
    /// Gets the logical drives available on the current machine.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.GetLogicalDrives()" />.</remarks>
    /// <returns>The logical drives available on the current machine.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DirectoryPath[] GetLogicalDrives()
        => Directory.GetLogicalDrives().Select(static x => new DirectoryPath(x)).ToArray();

    /// <summary>
    /// Gets the desktop directory for the current user.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder)" /> with <see cref="Environment.SpecialFolder.DesktopDirectory" />.</remarks>
    public static DirectoryPath Desktop
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
    }

    /// <summary>
    /// Gets the documents directory for the current user.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder)" /> with <see cref="Environment.SpecialFolder.MyDocuments" />.</remarks>
    public static DirectoryPath Documents
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    /// <summary>
    /// Gets the user profile directory for the current user.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder)" /> with <see cref="Environment.SpecialFolder.UserProfile" />.</remarks>
    public static DirectoryPath UserProfile
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetFolderPath(Environment.SpecialFolder.UserProfile);
    }

    /// <summary>
    /// Gets the roaming application data directory for the current user.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder)" /> with <see cref="Environment.SpecialFolder.ApplicationData" />.</remarks>
    public static DirectoryPath ApplicationData
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }

    /// <summary>
    /// Gets the local application data directory for the current user.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder)" /> with <see cref="Environment.SpecialFolder.LocalApplicationData" />.</remarks>
    public static DirectoryPath LocalApplicationData
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }

    /// <summary>
    /// Gets the shared application data directory for all users.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder)" /> with <see cref="Environment.SpecialFolder.CommonApplicationData" />.</remarks>
    public static DirectoryPath CommonApplicationData
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    }

    /// <summary>
    /// Gets the base directory for the current application context.
    /// </summary>
    /// <remarks>Wraps <see cref="AppContext.BaseDirectory" />.</remarks>
    public static DirectoryPath BaseDirectory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(AppContext.BaseDirectory);
    }

    /// <summary>
    /// Gets the temporary directory for the current system.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetTempPath()" />.</remarks>
    public static DirectoryPath Temp
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Path.GetTempPath());
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Creates a temporary subdirectory using the specified prefix.
    /// </summary>
    /// <remarks>Wraps <see cref="Directory.CreateTempSubdirectory(string)" />.</remarks>
    /// <param name="prefix">The prefix for the temporary directory name.</param>
    /// <returns>The created temporary directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DirectoryPath CreateTempSubdirectory(string prefix)
        => new(Directory.CreateTempSubdirectory(prefix).FullName);
#endif

    /// <summary>
    /// Gets the system directory for the current machine.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.SystemDirectory" />.</remarks>
    public static DirectoryPath System
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Environment.SystemDirectory);
    }

    /// <summary>
    /// Gets the path of the specified special folder.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder)" />.</remarks>
    /// <param name="folder">The special folder to resolve.</param>
    /// <returns>The resolved directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DirectoryPath GetFolderPath(Environment.SpecialFolder folder)
        => new(Environment.GetFolderPath(folder));

    /// <summary>
    /// Gets the path of the specified special folder using the requested verification option.
    /// </summary>
    /// <remarks>Wraps <see cref="Environment.GetFolderPath(Environment.SpecialFolder,Environment.SpecialFolderOption)" />.</remarks>
    /// <param name="folder">The special folder to resolve.</param>
    /// <param name="option">The verification option to apply.</param>
    /// <returns>The resolved directory path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DirectoryPath GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        => new(Environment.GetFolderPath(folder, option));
}
