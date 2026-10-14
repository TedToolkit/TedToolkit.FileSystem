// -----------------------------------------------------------------------
// <copyright file="ThrowIfContainsNoEntriesTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfContainsNoEntriesTests
{
    /// <summary>
    /// Verifies that a directory with no entries is rejected.
    /// </summary>
    [Test]
    public async Task Should_reject_folder_when_it_has_no_entries()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFolder = fixture.DirectoryPath;
        var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoEntries()).Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("inputFolder");
        await Assert.That(exception.Message).Contains(inputFolder.FullName);
    }

    /// <summary>
    /// Verifies that either a file or an empty subfolder makes a directory nonempty.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Should_accept_folder_when_it_has_an_entry(bool createFolder)
    {
        using var fixture = new FileSystemGuardFixture();
        if (createFolder)
        {
            fixture.CreateFolder();
        }
        else
        {
            fixture.CreateFile();
        }

        fixture.DirectoryPath.ThrowIfContainsNoEntries();
        await Assert.That(Directory.EnumerateFileSystemEntries(fixture.DirectoryPath.FullName).Any()).IsTrue();
    }

    /// <summary>
    /// Verifies that missing and null directory references fail before entry inspection.
    /// </summary>
    [Test]
    public async Task Should_validate_directory_before_entries()
    {
        using var fixture = new FileSystemGuardFixture();
        DirectoryPath? inputFolder = null;
        var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoEntries()).Throws<ArgumentNullException>();
        await Assert.That(exception!.ParamName).IsEqualTo("inputFolder");
        await Assert.That(() => fixture.MissingFolder.ThrowIfContainsNoEntries()).Throws<DirectoryNotFoundException>();
    }
}