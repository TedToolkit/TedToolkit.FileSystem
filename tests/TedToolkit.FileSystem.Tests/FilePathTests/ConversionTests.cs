// -----------------------------------------------------------------------
// <copyright file="ConversionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class ConversionTests
{
    /// <summary>
    /// Verifies that conversion helpers produce the expected file path values.
    /// </summary>
    [Test]
    public async Task Should_transform_file_paths_when_using_conversion_helpers()
    {
        var path = new FilePath(TestAssets.NestedFile.FullName);

        await Assert.That(path.FileName).IsEqualTo(new FileName("nested-file.txt"));
        await Assert.That(path.ToFileInfo().FullName).IsEqualTo(TestAssets.NestedFile.FullName);
        await Assert.That(path.GetFullPath().FullName).IsEqualTo(Path.GetFullPath(TestAssets.NestedFile.FullName));
        await Assert.That(path.ChangeExtension(".md").FullName).IsEqualTo(Path.ChangeExtension(TestAssets.NestedFile.FullName, ".md"));
        await Assert.That(path.WithExtension(".json").FullName).IsEqualTo(Path.ChangeExtension(TestAssets.NestedFile.FullName, ".json"));
        await Assert.That(path.WithFileName("renamed.txt").FullName).IsEqualTo(Path.Combine(TestAssets.NestedDirectory.FullName, "renamed.txt"));
        await Assert.That(path.GetRelativePath(TestAssets.RootDirectory.ToPath()).FullName).IsEqualTo(Path.GetRelativePath(TestAssets.RootDirectory.FullName, TestAssets.NestedFile.FullName));
    }

    /// <summary>
    /// Verifies that relative file paths can be resolved against a base directory.
    /// </summary>
    [Test]
    public async Task Should_resolve_relative_file_path_when_base_directory_is_supplied()
    {
        var path = new FilePath(Path.Combine("nested", "nested-file.txt"));

        await Assert.That(path.GetFullPath(TestAssets.RootDirectory.ToPath()).FullName)
            .IsEqualTo(Path.GetFullPath(path.FullName, TestAssets.RootDirectory.FullName));
        await Assert.That(path.GetRelativePath(TestAssets.RootDirectory.FullName).FullName)
            .IsEqualTo(Path.GetRelativePath(TestAssets.RootDirectory.FullName, path.FullName));
    }
}
