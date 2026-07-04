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
public readonly record struct FilePath(string FullName);