// -----------------------------------------------------------------------
// <copyright file="ThrowIfContainsNoFilesTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfContainsNoFilesTests
{
    /// <summary>
    /// Verifies that neither an empty folder nor a folder containing only subfolders contains files.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Should_reject_folder_when_it_contains_no_files(bool createChild)
    {
        using var fixture = new FileSystemGuardFixture();
        if (createChild)
        {
            fixture.CreateFolder();
        }

        var inputFolder = fixture.DirectoryPath;
        var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles()).Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("inputFolder");
        await Assert.That(exception.Message).Contains(inputFolder.FullName);
    }

    /// <summary>
    /// Verifies that a zero-byte file satisfies a file-presence check.
    /// </summary>
    [Test]
    public async Task Should_accept_folder_when_it_contains_a_zero_byte_file()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFile = fixture.CreateFile();
        fixture.DirectoryPath.ThrowIfContainsNoFiles();
        await Assert.That(inputFile.ToFileInfo().Length).IsEqualTo(0L);
    }

    /// <summary>
    /// Verifies that recursive search is explicitly requested for files inside child directories.
    /// </summary>
    [Test]
    public async Task Should_include_nested_files_only_when_recursion_is_requested()
    {
        using var fixture = new FileSystemGuardFixture();
        fixture.CreateFile("child/input.json");
        var inputFolder = fixture.DirectoryPath;
        await Assert.That(() => inputFolder.ThrowIfContainsNoFiles()).Throws<InvalidOperationException>();
        inputFolder.ThrowIfContainsNoFiles("*", SearchOption.AllDirectories);
    }

    /// <summary>
    /// Verifies that native wildcard matching distinguishes direct files, nested files, and nonmatching extensions.
    /// </summary>
    [Test]
    public async Task Should_apply_native_pattern_without_treating_unrelated_files_as_matches()
    {
        using var fixture = new FileSystemGuardFixture();
        fixture.CreateFile("other.csv");
        fixture.CreateFile("child/input.json");
        var inputFolder = fixture.DirectoryPath;
        var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles("*.json"))
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("*.json");
        await Assert.That(exception.Message).Contains("inputFolder");
        await Assert.That(exception.Message).Contains(inputFolder.FullName);
        inputFolder.ThrowIfContainsNoFiles("*.json", SearchOption.AllDirectories);
        inputFolder.ThrowIfContainsNoFiles("*.csv");
    }

    /// <summary>
    /// Verifies that native wildcard matching preserves the underlying case-insensitive matching behavior.
    /// </summary>
    [Test]
    public async Task Should_follow_platform_case_rules_when_extension_case_differs()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFile = fixture.CreateFile("input.JSON");
        if (OperatingSystem.IsWindows())
        {
            fixture.DirectoryPath.ThrowIfContainsNoFiles("*.json");
        }
        else
        {
            await Assert.That(() => fixture.DirectoryPath.ThrowIfContainsNoFiles("*.json"))
                .Throws<InvalidOperationException>();
        }

        await Assert.That(inputFile.Exists).IsTrue();
    }

    /// <summary>
    /// Verifies that invalid patterns report the caller's pattern argument.
    /// </summary>
    [Test]
    [Arguments((string?)null)]
    [Arguments("")]
    [Arguments(" \t ")]
    public async Task Should_report_pattern_argument_when_pattern_is_invalid(string? pattern)
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFolder = fixture.DirectoryPath;
        if (pattern is null)
        {
            var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles(pattern))
                .Throws<ArgumentNullException>();
            await Assert.That(exception!.ParamName).IsEqualTo("pattern");
        }
        else
        {
            var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles(pattern))
                .Throws<ArgumentException>();
            await Assert.That(exception!.ParamName).IsEqualTo("pattern");
        }
    }

    /// <summary>
    /// Verifies that null and missing directory failures take precedence over invalid search patterns.
    /// </summary>
    [Test]
    public async Task Should_validate_directory_before_pattern_or_contents()
    {
        using var fixture = new FileSystemGuardFixture();
        DirectoryPath? inputFolder = null;
        var exception = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles((string?)null))
            .Throws<ArgumentNullException>();
        await Assert.That(exception!.ParamName).IsEqualTo("inputFolder");
        await Assert.That(() => fixture.MissingFolder.ThrowIfContainsNoFiles((string?)null))
            .Throws<DirectoryNotFoundException>();
        await Assert.That(() => fixture.MissingFolder.ThrowIfContainsNoFiles())
            .Throws<DirectoryNotFoundException>();
    }

    /// <summary>
    /// Verifies that unsupported SearchOption values retain the framework's argument exception.
    /// </summary>
    [Test]
    public async Task Should_reject_unsupported_search_option()
    {
        using var fixture = new FileSystemGuardFixture();
        await Assert.That(() => fixture.DirectoryPath.ThrowIfContainsNoFiles("*", (SearchOption)42))
            .Throws<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies that a nullable directory uses native wildcard filtering and explicit recursive scope.
    /// </summary>
    [Test]
    public async Task Should_search_present_nullable_directory_values()
    {
        using var fixture = new FileSystemGuardFixture();
        fixture.CreateFile("child/input.json");
        DirectoryPath? inputDirectory = fixture.DirectoryPath;
        var exception = await Assert.That(() => inputDirectory.ThrowIfContainsNoFiles("*.json"))
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("inputDirectory");
        inputDirectory.ThrowIfContainsNoFiles("*.json", SearchOption.AllDirectories);
        inputDirectory.ThrowIfContainsNoFolders("child");
        inputDirectory.ThrowIfContainsNoEntries();
        await Assert.That(inputDirectory.Value.FullName).IsEqualTo(fixture.DirectoryPath.FullName);
    }
}