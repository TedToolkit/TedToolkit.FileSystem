// -----------------------------------------------------------------------
// <copyright file="FileSystemGuardFixture.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class FileSystemGuardFixture : IDisposable
{
    private readonly string root = Path.Combine(Path.GetTempPath(), $"tedtoolkit-guards-{Guid.NewGuid():N}");

    internal FileSystemGuardFixture()
    {
        Directory.CreateDirectory(root);
    }

    internal DirectoryPath DirectoryPath
    {
        get
        {
            return new(root);
        }
    }

    internal FilePath CreateFile(string relativePath = "input.json")
    {
        var path = Path.Combine(root, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "");
        return new(path);
    }

    internal DirectoryPath CreateFolder(string relativePath = "child")
    {
        var path = Path.Combine(root, relativePath);
        Directory.CreateDirectory(path);
        return new(path);
    }

    internal FilePath MissingFile
    {
        get
        {
            return new(Path.Combine(root, "missing.json"));
        }
    }

    internal DirectoryPath MissingFolder
    {
        get
        {
            return new(Path.Combine(root, "missing"));
        }
    }

    public void Dispose()
    {
        Directory.Delete(root, recursive: true);
    }
}