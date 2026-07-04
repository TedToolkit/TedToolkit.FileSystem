// -----------------------------------------------------------------------
// <copyright file="DirectoryPath.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Represents a directory path value.
/// </summary>
/// <param name="FullName">The full directory path text.</param>
public readonly record struct DirectoryPath(string FullName);