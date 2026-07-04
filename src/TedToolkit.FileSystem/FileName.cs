// -----------------------------------------------------------------------
// <copyright file="FileName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem;

/// <summary>
/// Represents a file name value.
/// </summary>
/// <param name="Name">The file name text.</param>
public readonly record struct FileName(string Name);
