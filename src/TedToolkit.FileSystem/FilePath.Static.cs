// -----------------------------------------------------------------------
// <copyright file="FilePath.Static.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides static file-scoped members for <see cref="FilePath" />.
/// </summary>
public readonly partial record struct FilePath
{
    /// <summary>
    /// Creates a temporary file and returns its path.
    /// </summary>
    /// <remarks>Wraps <see cref="Path.GetTempFileName()" />.</remarks>
    /// <returns>The created temporary file path.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FilePath GetTempFileName()
    {
        return new(Path.GetTempFileName());
    }

    /// <summary>
    /// Creates a file path value object from an assembly location.
    /// </summary>
    /// <remarks>Uses the <see cref="Assembly.Location" /> value from the supplied <see cref="Assembly" /> instance.</remarks>
    /// <param name="assembly">The assembly to convert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="assembly" /> is <see langword="null" />.</exception>
    /// <returns>A file path value object that uses the assembly location.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FilePath FromAssembly(Assembly assembly)
    {
        return new((assembly ?? throw new ArgumentNullException(nameof(assembly))).Location);
    }

    /// <summary>
    /// Gets a file path value object from the entry assembly location when available.
    /// </summary>
    /// <remarks>Wraps <see cref="Assembly.GetEntryAssembly()" /> and uses <see cref="Assembly.Location" /> when a value is returned.</remarks>
    public static FilePath? EntryAssembly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return Assembly.GetEntryAssembly() is { } assembly ? new FilePath(assembly.Location) : default(FilePath?);
        }
    }

    /// <summary>
    /// Gets a file path value object from the currently executing assembly location.
    /// </summary>
    /// <remarks>Wraps <see cref="Assembly.GetExecutingAssembly()" /> and uses <see cref="Assembly.Location" />.</remarks>
    public static FilePath ExecutingAssembly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return new(Assembly.GetExecutingAssembly().Location);
        }
    }

    /// <summary>
    /// Gets a file path value object from the calling assembly location.
    /// </summary>
    /// <remarks>Wraps <see cref="Assembly.GetCallingAssembly()" /> and uses <see cref="Assembly.Location" />.</remarks>
    public static FilePath CallingAssembly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return new(Assembly.GetCallingAssembly().Location);
        }
    }
}