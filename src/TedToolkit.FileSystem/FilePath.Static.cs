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
    public static FilePath GetTempFileName()
    {
        return new(Path.GetTempFileName());
    }

    /// <summary>
    /// Creates a file path value object from an assembly location.
    /// </summary>
    /// <remarks>Uses the <see cref="Assembly.Location" /> value from the supplied <see cref="Assembly" /> instance.</remarks>
    /// <param name="assembly">The assembly to convert.</param>
    /// <returns>A file path value object that uses the assembly location.</returns>
    public static FilePath FromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        return new(assembly.Location);
    }

    /// <summary>
    /// Creates a file path value object from the entry assembly location when available.
    /// </summary>
    /// <remarks>Wraps <see cref="Assembly.GetEntryAssembly()" /> and uses <see cref="Assembly.Location" /> when a value is returned.</remarks>
    /// <returns>
    /// A file path value object that uses the entry assembly location, or <see langword="null" />
    /// when the current process has no entry assembly.
    /// </returns>
    public static FilePath? FromEntryAssembly()
    {
        var assembly = Assembly.GetEntryAssembly();
        return assembly is null ? default(FilePath?) : FromAssembly(assembly);
    }

    /// <summary>
    /// Creates a file path value object from the currently executing assembly location.
    /// </summary>
    /// <remarks>Wraps <see cref="Assembly.GetExecutingAssembly()" /> and uses <see cref="Assembly.Location" />.</remarks>
    /// <returns>A file path value object that uses the executing assembly location.</returns>
    public static FilePath FromExecutingAssembly()
    {
        return FromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Creates a file path value object from the calling assembly location.
    /// </summary>
    /// <remarks>Wraps <see cref="Assembly.GetCallingAssembly()" /> and uses <see cref="Assembly.Location" />.</remarks>
    /// <returns>A file path value object that uses the calling assembly location.</returns>
    public static FilePath FromCallingAssembly()
    {
        return FromAssembly(Assembly.GetCallingAssembly());
    }
}
