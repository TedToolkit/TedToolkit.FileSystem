// -----------------------------------------------------------------------
// <copyright file="EnumerationTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.DirectoryPathTests;

internal sealed class EnumerationTests
{
    [Test]
    public async Task Should_enumerate_directories_through_all_public_overloads()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var recursiveOptions = new() { RecurseSubdirectories = true, };

        await Assert.That(path.EnumerateDirectories().Select(x => x.FullName).Order().ToArray())
            .IsEquivalentTo([
                TestAssets.NestedDirectory.FullName,
                TestAssets.SiblingDirectory.FullName,
            ]);

        await Assert.That(path.EnumerateDirectories("nested").Single())
            .IsEqualTo(new DirectoryPath(TestAssets.NestedDirectory.FullName));

        await Assert.That(path.EnumerateDirectories("*", SearchOption.AllDirectories).Count()).IsEqualTo(2);
        await Assert.That(path.EnumerateDirectories("*", recursiveOptions).Count()).IsEqualTo(2);
    }

    [Test]
    public async Task Should_get_directories_through_all_public_overloads()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var recursiveOptions = new() { RecurseSubdirectories = true, };

        await Assert.That(path.GetDirectories().Select(x => x.FullName).Order().ToArray())
            .IsEquivalentTo([
                TestAssets.NestedDirectory.FullName,
                TestAssets.SiblingDirectory.FullName,
            ]);

        await Assert.That(path.GetDirectories("nested").Single())
            .IsEqualTo(new DirectoryPath(TestAssets.NestedDirectory.FullName));

        await Assert.That(path.GetDirectories("*", SearchOption.AllDirectories).Length).IsEqualTo(2);
        await Assert.That(path.GetDirectories("*", recursiveOptions).Length).IsEqualTo(2);
    }

    [Test]
    public async Task Should_enumerate_files_through_all_public_overloads()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var recursiveOptions = new() { RecurseSubdirectories = true, };

        await Assert.That(path.EnumerateFiles().Single())
            .IsEqualTo(new FilePath(TestAssets.RootFile.FullName));

        await Assert.That(path.EnumerateFiles("root-file.txt").Single())
            .IsEqualTo(new FilePath(TestAssets.RootFile.FullName));

        await Assert.That(path.EnumerateFiles("*", SearchOption.AllDirectories).OrderBy(x => x.FullName).Select(x => x.FullName).ToArray())
            .IsEquivalentTo([
                TestAssets.NestedFile.FullName,
                TestAssets.RootFile.FullName,
                TestAssets.SiblingFile.FullName,
            ]);

        await Assert.That(path.EnumerateFiles("*", recursiveOptions).Count()).IsEqualTo(3);
    }

    [Test]
    public async Task Should_get_files_through_all_public_overloads()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var recursiveOptions = new() { RecurseSubdirectories = true, };

        await Assert.That(path.GetFiles().Single())
            .IsEqualTo(new FilePath(TestAssets.RootFile.FullName));

        await Assert.That(path.GetFiles("root-file.txt").Single())
            .IsEqualTo(new FilePath(TestAssets.RootFile.FullName));

        await Assert.That(path.GetFiles("*", SearchOption.AllDirectories).OrderBy(x => x.FullName).Select(x => x.FullName).ToArray())
            .IsEquivalentTo([
                TestAssets.NestedFile.FullName,
                TestAssets.RootFile.FullName,
                TestAssets.SiblingFile.FullName,
            ]);

        await Assert.That(path.GetFiles("*", recursiveOptions).Length).IsEqualTo(3);
    }

    [Test]
    public async Task Should_enumerate_file_system_entries_through_all_public_overloads()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var recursiveOptions = new() { RecurseSubdirectories = true, };

        await Assert.That(path.EnumerateFileSystemEntries().Order().ToArray())
            .IsEquivalentTo([
                TestAssets.NestedDirectory.FullName,
                TestAssets.RootFile.FullName,
                TestAssets.SiblingDirectory.FullName,
            ]);

        await Assert.That(path.EnumerateFileSystemEntries("root-file.txt").Single())
            .IsEqualTo(TestAssets.RootFile.FullName);

        await Assert.That(path.EnumerateFileSystemEntries("*", SearchOption.AllDirectories).Count()).IsEqualTo(5);
        await Assert.That(path.EnumerateFileSystemEntries("*", recursiveOptions).Count()).IsEqualTo(5);
    }

    [Test]
    public async Task Should_get_file_system_entries_through_all_public_overloads()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var recursiveOptions = new() { RecurseSubdirectories = true, };

        await Assert.That(path.GetFileSystemEntries().Order().ToArray())
            .IsEquivalentTo([
                TestAssets.NestedDirectory.FullName,
                TestAssets.RootFile.FullName,
                TestAssets.SiblingDirectory.FullName,
            ]);

        await Assert.That(path.GetFileSystemEntries("root-file.txt").Single())
            .IsEqualTo(TestAssets.RootFile.FullName);

        await Assert.That(path.GetFileSystemEntries("*", SearchOption.AllDirectories).Length).IsEqualTo(5);
        await Assert.That(path.GetFileSystemEntries("*", recursiveOptions).Length).IsEqualTo(5);
    }
}
