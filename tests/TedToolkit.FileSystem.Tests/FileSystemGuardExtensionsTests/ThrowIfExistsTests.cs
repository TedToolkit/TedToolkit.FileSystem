// -----------------------------------------------------------------------
// <copyright file="ThrowIfExistsTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfExistsTests
{
    /// <summary>
    /// Verifies that occupied paths are rejected even when their entry kind differs.
    /// </summary>
    [Test]
    public async Task Should_reject_occupied_paths_for_both_entry_kinds()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFile = fixture.CreateFile();
        var inputFolder = fixture.DirectoryPath;
        var fileAtFolder = new FilePath(inputFolder.FullName);
        var folderAtFile = new DirectoryPath(inputFile.FullName);
        var exception = await Assert.That(() => inputFile.ThrowIfExists()).Throws<IOException>();
        await Assert.That(exception!.Message).Contains("inputFile");
        await Assert.That(exception.Message).Contains(inputFile.FullName);
        await Assert.That(() => inputFolder.ThrowIfExists()).Throws<IOException>();
        await Assert.That(() => fileAtFolder.ThrowIfExists()).Throws<IOException>();
        await Assert.That(() => folderAtFile.ThrowIfExists()).Throws<IOException>();
    }

    /// <summary>
    /// Verifies that available paths pass without creating an entry.
    /// </summary>
    [Test]
    public async Task Should_accept_available_paths_without_creating_entries()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFile = fixture.MissingFile;
        var inputFolder = fixture.MissingFolder;
        inputFile.ThrowIfExists();
        inputFolder.ThrowIfExists();
        await Assert.That(inputFile.Exists).IsFalse();
        await Assert.That(inputFolder.Exists).IsFalse();
    }

    /// <summary>
    /// Verifies that null output references are rejected with their caller names.
    /// </summary>
    [Test]
    public async Task Should_reject_null_output_references()
    {
        FilePath? outputFile = null;
        DirectoryPath? outputFolder = null;
        var fileException = await Assert.That(() => outputFile.ThrowIfExists()).Throws<ArgumentNullException>();
        var folderException = await Assert.That(() => outputFolder.ThrowIfExists()).Throws<ArgumentNullException>();
        await Assert.That(fileException!.ParamName).IsEqualTo("outputFile");
        await Assert.That(folderException!.ParamName).IsEqualTo("outputFolder");
    }
}