// -----------------------------------------------------------------------
// <copyright file="PathPropertiesTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.DirectoryPathTests;

internal sealed class PathPropertiesTests
{
    [Test]
    public async Task Should_expose_directory_name_through_name_property()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        await Assert.That(path.Name).IsEqualTo("nested");
    }

    [Test]
    public async Task Should_expose_parent_directory_through_parent_property()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        await Assert.That(path.Parent).IsEqualTo(new DirectoryPath(TestAssets.RootDirectory.FullName));
    }

    [Test]
    public async Task Should_expose_root_directory_through_root_property()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        await Assert.That(path.Root).IsEqualTo(new DirectoryPath(Path.GetPathRoot(TestAssets.NestedDirectory.FullName)!));
    }

    [Test]
    public async Task Should_expose_extension_through_extension_property()
    {
        var path = new DirectoryPath(@"C:\temp\archive.v1");

        await Assert.That(path.Extension).IsEqualTo(".v1");
    }

    [Test]
    public async Task Should_report_when_directory_path_has_extension()
    {
        var path = new DirectoryPath(@"C:\temp\archive.v1");

        await Assert.That(path.HasExtension).IsTrue();
    }

    [Test]
    public async Task Should_report_when_directory_path_is_rooted()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        await Assert.That(path.IsPathRooted).IsTrue();
    }

    [Test]
    public async Task Should_report_when_directory_path_ends_with_directory_separator()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName + Path.DirectorySeparatorChar);

        await Assert.That(path.EndsInDirectorySeparator).IsTrue();
    }

    [Test]
    public async Task Should_return_normalized_full_path()
    {
        var originalCurrentDirectory = Environment.CurrentDirectory;
        var relativePath = new DirectoryPath(Path.Combine(".", "assets", "nested"));

        try
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(TestAssets.RootDirectory.FullName)!;

            var fullPath = relativePath.GetFullPath();

            await Assert.That(fullPath).IsEqualTo(new DirectoryPath(Path.GetFullPath(relativePath.FullName)));
        }
        finally
        {
            Environment.CurrentDirectory = originalCurrentDirectory;
        }
    }

    [Test]
    public async Task Should_return_full_path_relative_to_base_path()
    {
        var relativePath = new DirectoryPath(Path.Combine("nested", "child"));
        var basePath = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var fullPath = relativePath.GetFullPath(basePath);

        await Assert.That(fullPath).IsEqualTo(new DirectoryPath(Path.GetFullPath(relativePath.FullName, basePath.FullName)));
    }

    [Test]
    public async Task Should_trim_ending_directory_separator()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName + Path.DirectorySeparatorChar);

        var trimmedPath = path.TrimEndingDirectorySeparator();

        await Assert.That(trimmedPath).IsEqualTo(new DirectoryPath(Path.TrimEndingDirectorySeparator(path.FullName)));
    }

    [Test]
    public async Task Should_change_extension()
    {
        var path = new DirectoryPath(@"C:\temp\archive.v1");

        var changedPath = path.ChangeExtension(".v2");

        await Assert.That(changedPath).IsEqualTo(new DirectoryPath(Path.ChangeExtension(path.FullName, ".v2")!));
    }

    [Test]
    public async Task Should_get_relative_path_to_target_directory()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var target = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        var relativePath = path.GetRelativePathTo(target);

        await Assert.That(relativePath).IsEqualTo(Path.GetRelativePath(path.FullName, target.FullName));
    }
}