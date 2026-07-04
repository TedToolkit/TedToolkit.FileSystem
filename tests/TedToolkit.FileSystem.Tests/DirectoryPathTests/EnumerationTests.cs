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
    public async Task Should_enumerate_directories()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var directories = path.EnumerateDirectories().OrderBy(x => x.FullName).ToArray();

        await Assert.That(directories.Length).IsEqualTo(2);
        await Assert.That(directories[0]).IsEqualTo(new DirectoryPath(TestAssets.NestedDirectory.FullName));
        await Assert.That(directories[1]).IsEqualTo(new DirectoryPath(TestAssets.SiblingDirectory.FullName));
    }

    [Test]
    public async Task Should_enumerate_directories_with_search_pattern_and_option()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var directories = path.EnumerateDirectories("*", SearchOption.AllDirectories).OrderBy(x => x.FullName).ToArray();

        await Assert.That(directories.Length).IsEqualTo(2);
    }

    [Test]
    public async Task Should_enumerate_directories_with_enumeration_options()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
        };

        var directories = path.EnumerateDirectories("*", options).OrderBy(x => x.FullName).ToArray();

        await Assert.That(directories.Length).IsEqualTo(2);
    }

    [Test]
    public async Task Should_get_directories_as_array()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var directories = path.GetDirectories("nested");

        await Assert.That(directories.Length).IsEqualTo(1);
        await Assert.That(directories[0]).IsEqualTo(new DirectoryPath(TestAssets.NestedDirectory.FullName));
    }

    [Test]
    public async Task Should_get_directories_as_array_with_enumeration_options()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
        };

        var directories = path.GetDirectories("*", options).OrderBy(x => x.FullName).ToArray();

        await Assert.That(directories.Length).IsEqualTo(2);
    }

    [Test]
    public async Task Should_enumerate_files()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var files = path.EnumerateFiles("*", SearchOption.AllDirectories).OrderBy(x => x.FullName).ToArray();

        await Assert.That(files.Length).IsEqualTo(3);
        await Assert.That(files[0]).IsEqualTo(new FilePath(TestAssets.NestedFile.FullName));
        await Assert.That(files[1]).IsEqualTo(new FilePath(TestAssets.RootFile.FullName));
        await Assert.That(files[2]).IsEqualTo(new FilePath(TestAssets.SiblingFile.FullName));
    }

    [Test]
    public async Task Should_enumerate_files_with_enumeration_options()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
        };

        var files = path.EnumerateFiles("*", options).OrderBy(x => x.FullName).ToArray();

        await Assert.That(files.Length).IsEqualTo(3);
    }

    [Test]
    public async Task Should_get_files_as_array()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        var files = path.GetFiles("*.txt");

        await Assert.That(files.Length).IsEqualTo(1);
        await Assert.That(files[0]).IsEqualTo(new FilePath(TestAssets.NestedFile.FullName));
    }

    [Test]
    public async Task Should_get_files_as_array_with_enumeration_options()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
        };

        var files = path.GetFiles("*", options).OrderBy(x => x.FullName).ToArray();

        await Assert.That(files.Length).IsEqualTo(3);
    }

    [Test]
    public async Task Should_enumerate_file_system_entries()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var entries = path.EnumerateFileSystemEntries().OrderBy(x => x).ToArray();

        await Assert.That(entries.Length).IsEqualTo(3);
        await Assert.That(entries[0]).IsEqualTo(TestAssets.NestedDirectory.FullName);
        await Assert.That(entries[1]).IsEqualTo(TestAssets.RootFile.FullName);
        await Assert.That(entries[2]).IsEqualTo(TestAssets.SiblingDirectory.FullName);
    }

    [Test]
    public async Task Should_enumerate_file_system_entries_with_enumeration_options()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
        };

        var entries = path.EnumerateFileSystemEntries("*", options).OrderBy(x => x).ToArray();

        await Assert.That(entries.Length).IsEqualTo(5);
    }

    [Test]
    public async Task Should_get_file_system_entries_as_array()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var entries = path.GetFileSystemEntries("*", SearchOption.AllDirectories).OrderBy(x => x).ToArray();

        await Assert.That(entries.Length).IsEqualTo(5);
    }

    [Test]
    public async Task Should_get_file_system_entries_as_array_with_enumeration_options()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
        };

        var entries = path.GetFileSystemEntries("*", options).OrderBy(x => x).ToArray();

        await Assert.That(entries.Length).IsEqualTo(5);
    }
}
