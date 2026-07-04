// -----------------------------------------------------------------------
// <copyright file="DerivedPropertyTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class DerivedPropertyTests
{
    /// <summary>
    /// Verifies that file-specific path values are exposed through derived properties.
    /// </summary>
    [Test]
    public async Task Should_expose_derived_path_properties_when_file_path_has_parent_and_extension()
    {
        var path = new FilePath(TestAssets.NestedFile.FullName);

        await Assert.That(path.Name).IsEqualTo("nested-file.txt");
        await Assert.That(path.NameWithoutExtension).IsEqualTo("nested-file");
        await Assert.That(path.Extension).IsEqualTo(".txt");
        await Assert.That(path.ParentDirectory).IsEqualTo(new DirectoryPath(TestAssets.NestedDirectory.FullName));
        await Assert.That(path.Root).IsEqualTo(Path.GetPathRoot(TestAssets.NestedFile.FullName));
        await Assert.That(path.HasExtension).IsTrue();
        await Assert.That(path.IsPathRooted).IsTrue();
        await Assert.That(path.IsFullyQualified).IsTrue();
        await Assert.That(path.Exists).IsTrue();
    }

    /// <summary>
    /// Verifies that file metadata values are exposed through properties.
    /// </summary>
    [Test]
    public async Task Should_expose_file_metadata_properties_when_file_exists()
    {
        var file = TestWorkspace.CreateFile("metadata.txt", "hello");
        var path = new FilePath(file.FullName);

        try
        {
            await Assert.That(path.Length).IsEqualTo(file.Length);
            await Assert.That(path.CreationTime).IsEqualTo(File.GetCreationTime(file.FullName));
            await Assert.That(path.CreationTimeUtc).IsEqualTo(File.GetCreationTimeUtc(file.FullName));
            await Assert.That(path.LastWriteTime).IsEqualTo(File.GetLastWriteTime(file.FullName));
            await Assert.That(path.LastWriteTimeUtc).IsEqualTo(File.GetLastWriteTimeUtc(file.FullName));
            await Assert.That(path.LastAccessTime).IsEqualTo(File.GetLastAccessTime(file.FullName));
            await Assert.That(path.LastAccessTimeUtc).IsEqualTo(File.GetLastAccessTimeUtc(file.FullName));
            await Assert.That(path.Attributes).IsEqualTo(File.GetAttributes(file.FullName));
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }
}
