// -----------------------------------------------------------------------
// <copyright file="ThrowIfContainsNoFoldersTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfContainsNoFoldersTests
{
    /// <summary>
    /// Verifies that files do not satisfy a subfolder-presence check.
    /// </summary>
    [Test]
    public async Task Should_reject_folder_when_it_contains_only_files()
    {
        using var fixture = new FileSystemGuardFixture();
        fixture.CreateFile();
        var inputFolder = fixture.DirectoryPath;
        var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoFolders()).Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("inputFolder");
        await Assert.That(exception.Message).Contains(inputFolder.FullName);
    }

    /// <summary>
    /// Verifies that an empty child directory satisfies a subfolder-presence check.
    /// </summary>
    [Test]
    public async Task Should_accept_folder_when_it_contains_an_empty_child_folder()
    {
        using var fixture = new FileSystemGuardFixture();
        var child = fixture.CreateFolder();
        fixture.DirectoryPath.ThrowIfContainsNoFolders();
        fixture.DirectoryPath.ThrowIfContainsNoFolders("*", SearchOption.AllDirectories);
        await Assert.That(child.Exists).IsTrue();
    }

    /// <summary>
    /// Verifies that unrelated child folders do not satisfy the requested pattern.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Should_reject_folder_when_no_child_matches_pattern(bool useNullablePath)
    {
        using var fixture = new FileSystemGuardFixture();
        fixture.CreateFolder("logs");
        fixture.CreateFolder("cache");
        var inputFolder = fixture.DirectoryPath;
        DirectoryPath? nullableFolder = inputFolder;

        if (useNullablePath)
        {
            await Assert.That(() => nullableFolder.ThrowIfContainsNoFolders("images*")).Throws<InvalidOperationException>();
        }
        else
        {
            await Assert.That(() => inputFolder.ThrowIfContainsNoFolders("images*")).Throws<InvalidOperationException>();
        }
    }

    /// <summary>
    /// Verifies that a descendant-only matching folder requires recursive search.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Should_find_matching_descendant_only_when_search_is_recursive(bool useNullablePath)
    {
        using var fixture = new FileSystemGuardFixture();
        var descendant = fixture.CreateFolder(Path.Combine("parent", "matching-child"));
        var inputFolder = fixture.DirectoryPath;
        DirectoryPath? nullableFolder = inputFolder;

        if (useNullablePath)
        {
            await Assert.That(() => nullableFolder.ThrowIfContainsNoFolders("matching*")).Throws<InvalidOperationException>();
            nullableFolder.ThrowIfContainsNoFolders("matching*", SearchOption.AllDirectories);
        }
        else
        {
            await Assert.That(() => inputFolder.ThrowIfContainsNoFolders("matching*")).Throws<InvalidOperationException>();
            inputFolder.ThrowIfContainsNoFolders("matching*", SearchOption.AllDirectories);
        }

        await Assert.That(descendant.Exists).IsTrue();
    }

    /// <summary>
    /// Verifies that missing and null directory references fail before enumeration.
    /// </summary>
    [Test]
    public async Task Should_validate_directory_before_subfolders()
    {
        using var fixture = new FileSystemGuardFixture();
        DirectoryPath? inputFolder = null;
        var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoFolders()).Throws<ArgumentNullException>();
        await Assert.That(exception!.ParamName).IsEqualTo("inputFolder");
        await Assert.That(() => fixture.MissingFolder.ThrowIfContainsNoFolders()).Throws<DirectoryNotFoundException>();
        await Assert.That(() => fixture.DirectoryPath.ThrowIfContainsNoFolders()).Throws<InvalidOperationException>();
        await Assert.That(() => fixture.DirectoryPath.ThrowIfContainsNoFolders("*", (SearchOption)42))
            .Throws<ArgumentOutOfRangeException>();
    }
}