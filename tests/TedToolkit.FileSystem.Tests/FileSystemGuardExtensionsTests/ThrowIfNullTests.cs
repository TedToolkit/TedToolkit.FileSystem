// -----------------------------------------------------------------------
// <copyright file="ThrowIfNullTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfNullTests
{
    /// <summary>
    /// Verifies that a null file reports the caller's local variable.
    /// </summary>
    [Test]
    public async Task Should_capture_file_variable_when_reference_is_null()
    {
        FilePath? inputFile = null;
        var exception = await Assert.That(() => inputFile.ThrowIfNull()).Throws<ArgumentNullException>();
        await Assert.That(exception!.ParamName).IsEqualTo("inputFile");
    }

    /// <summary>
    /// Verifies that a null folder reports the caller's member expression.
    /// </summary>
    [Test]
    public async Task Should_capture_folder_member_when_reference_is_null()
    {
        var inputs = new Inputs();
        var exception = await Assert.That(() => inputs.DirectoryPath.ThrowIfNull()).Throws<ArgumentNullException>();
        await Assert.That(exception!.ParamName).IsEqualTo("inputs.DirectoryPath");
    }

    /// <summary>
    /// Verifies that an explicit name takes precedence over automatic capture.
    /// </summary>
    [Test]
    public async Task Should_use_explicit_name_when_supplied()
    {
        FilePath? inputFile = null;
        var exception = await Assert.That(() => inputFile.ThrowIfNull("source")).Throws<ArgumentNullException>();
        await Assert.That(exception!.ParamName).IsEqualTo("source");
    }

    /// <summary>
    /// Verifies that non-null references do not require an existing path.
    /// </summary>
    [Test]
    public async Task Should_accept_missing_paths_when_references_are_not_null()
    {
        using var fixture = new FileSystemGuardFixture();
        FilePath? inputFile = fixture.MissingFile;
        DirectoryPath? inputFolder = fixture.MissingFolder;
        inputFile.ThrowIfNull();
        inputFolder.ThrowIfNull();
        await Assert.That(inputFile.Value.FullName).IsEqualTo(fixture.MissingFile.FullName);
        await Assert.That(inputFolder.Value.FullName).IsEqualTo(fixture.MissingFolder.FullName);
    }

    private sealed class Inputs
    {
        public DirectoryPath? DirectoryPath { get; init; }
    }
}