// -----------------------------------------------------------------------
// <copyright file="TestAssets.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Sourcy;

namespace TedToolkit.FileSystem.Tests;

internal static class TestAssets
{
    private static readonly DirectoryInfo AssetRootDirectory = new(Path.Combine(
        Git.RootDirectory.FullName,
        "assets"));

    public static readonly DirectoryInfo RootDirectory = AssetRootDirectory;

    public static readonly DirectoryInfo NestedDirectory = new(Path.Combine(AssetRootDirectory.FullName, "nested"));

    public static readonly DirectoryInfo SiblingDirectory = new(Path.Combine(AssetRootDirectory.FullName, "sibling"));

    public static readonly FileInfo RootFile = new(Path.Combine(AssetRootDirectory.FullName, "root-file.txt"));

    public static readonly FileInfo NestedFile = new(Path.Combine(NestedDirectory.FullName, "nested-file.txt"));

    public static readonly FileInfo SiblingFile = new(Path.Combine(SiblingDirectory.FullName, "sibling-file.txt"));
}
