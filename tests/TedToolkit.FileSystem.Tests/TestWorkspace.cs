// -----------------------------------------------------------------------
// <copyright file="TestWorkspace.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Sourcy;

namespace TedToolkit.FileSystem.Tests;

internal static class TestWorkspace
{
    private static readonly DirectoryInfo RootDirectory = new(Path.Combine(
        Git.RootDirectory.FullName,
        ".tmp-tests"));

    public static DirectoryInfo CreateDirectory()
    {
        var directory = new DirectoryInfo(Path.Combine(RootDirectory.FullName, Guid.NewGuid().ToString("N")));
        directory.Create();
        return directory;
    }

    public static FileInfo CreateFile(string fileName, string? contents = null)
    {
        var directory = CreateDirectory();
        var file = new FileInfo(Path.Combine(directory.FullName, fileName));
        Directory.CreateDirectory(file.DirectoryName!);
        File.WriteAllText(file.FullName, contents ?? string.Empty);
        return file;
    }
}
