// -----------------------------------------------------------------------
// <copyright file="ThrowIfNotExistsTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfNotExistsTests
{
    /// <summary>
    /// Verifies that missing files include the caller expression and absolute path.
    /// </summary>
    [Test]
    public async Task Should_report_path_and_expression_when_file_is_missing()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFile = fixture.MissingFile;
        var exception = await Assert.That(() => inputFile.ThrowIfNotExists()).Throws<FileNotFoundException>();
        await Assert.That(exception!.FileName).IsEqualTo(inputFile.FullName);
        await Assert.That(exception.Message).Contains("inputFile");
        await Assert.That(exception.Message).Contains(inputFile.FullName);
    }

    /// <summary>
    /// Verifies that missing folders use the directory-specific exception.
    /// </summary>
    [Test]
    public async Task Should_report_path_and_expression_when_folder_is_missing()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFolder = fixture.MissingFolder;
        var exception = await Assert.That(() => inputFolder.ThrowIfNotExists()).Throws<DirectoryNotFoundException>();
        await Assert.That(exception!.Message).Contains("inputFolder");
        await Assert.That(exception.Message).Contains(inputFolder.FullName);
    }

    /// <summary>
    /// Verifies that diagnostics resolve relative missing paths without changing the stored values.
    /// </summary>
    /// <remarks>Runs alone because existing tests mutate the process current directory.</remarks>
    [Test]
    [NotInParallel]
    public async Task Should_report_absolute_paths_for_relative_missing_entries()
    {
        var relativeDirectory = $"missing-guards-{Guid.NewGuid():N}";
        var relativeFile = Path.Combine(relativeDirectory, "input.json");
        var inputFile = new FilePath(relativeFile);
        var inputFolder = new DirectoryPath(relativeDirectory);
        var absoluteFile = Path.Combine(Environment.CurrentDirectory, relativeFile);
        var absoluteFolder = Path.Combine(Environment.CurrentDirectory, relativeDirectory);

        var fileException = await Assert.That(() => inputFile.ThrowIfNotExists()).Throws<FileNotFoundException>();
        var folderException = await Assert.That(() => inputFolder.ThrowIfNotExists()).Throws<DirectoryNotFoundException>();

        await Assert.That(fileException!.FileName).IsEqualTo(absoluteFile);
        await Assert.That(fileException.Message).Contains(absoluteFile);
        await Assert.That(fileException.Message).Contains("inputFile");
        await Assert.That(folderException!.Message).Contains(absoluteFolder);
        await Assert.That(folderException.Message).Contains("inputFolder");
        await Assert.That(inputFile.FullName).IsEqualTo(relativeFile);
        await Assert.That(inputFolder.FullName).IsEqualTo(relativeDirectory);
    }

    /// <summary>
    /// Verifies that null references fail before existence is checked.
    /// </summary>
    [Test]
    public async Task Should_report_null_reference_before_missing_path()
    {
        FilePath? inputFile = null;
        DirectoryPath? inputFolder = null;
        var fileException = await Assert.That(() => inputFile.ThrowIfNotExists()).Throws<ArgumentNullException>();
        var folderException = await Assert.That(() => inputFolder.ThrowIfNotExists()).Throws<ArgumentNullException>();
        await Assert.That(fileException!.ParamName).IsEqualTo("inputFile");
        await Assert.That(folderException!.ParamName).IsEqualTo("inputFolder");
    }

    /// <summary>
    /// Verifies that existence checks distinguish files from directories.
    /// </summary>
    [Test]
    public async Task Should_reject_existing_path_when_entry_kind_does_not_match()
    {
        using var fixture = new FileSystemGuardFixture();
        var fileAtFolder = new FilePath(fixture.DirectoryPath.FullName);
        var folderAtFile = new DirectoryPath(fixture.CreateFile().FullName);
        await Assert.That(() => fileAtFolder.ThrowIfNotExists()).Throws<FileNotFoundException>();
        await Assert.That(() => folderAtFile.ThrowIfNotExists()).Throws<DirectoryNotFoundException>();
    }

    /// <summary>
    /// Verifies that existing files and folders satisfy the guard.
    /// </summary>
    [Test]
    public async Task Should_accept_existing_files_and_folders()
    {
        using var fixture = new FileSystemGuardFixture();
        var inputFile = fixture.CreateFile();
        var inputFolder = fixture.DirectoryPath;
        inputFile.ThrowIfNotExists();
        inputFolder.ThrowIfNotExists();
        await Assert.That(inputFile.Exists).IsTrue();
        await Assert.That(inputFolder.Exists).IsTrue();
    }

    /// <summary>
    /// Verifies that present nullable path values retain non-null flow after existence checks.
    /// </summary>
    [Test]
    public async Task Should_accept_present_nullable_path_values()
    {
        using var fixture = new FileSystemGuardFixture();
        FilePath? inputFile = fixture.CreateFile();
        DirectoryPath? inputDirectory = fixture.DirectoryPath;
        inputFile.ThrowIfNotExists();
        inputDirectory.ThrowIfNotExists();
        await Assert.That(inputFile.Value.FullName).IsEqualTo(fixture.DirectoryPath.Combine(new FileName("input.json")).FullName);
        await Assert.That(inputDirectory.Value.FullName).IsEqualTo(fixture.DirectoryPath.FullName);
    }
}