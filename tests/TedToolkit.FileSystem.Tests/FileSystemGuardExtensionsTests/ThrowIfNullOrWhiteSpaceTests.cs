// -----------------------------------------------------------------------
// <copyright file="ThrowIfNullOrWhiteSpaceTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfNullOrWhiteSpaceTests
{
    /// <summary>
    /// Verifies that default path values report the original caller expression.
    /// </summary>
    [Test]
    public async Task Should_reject_default_path_values()
    {
        FilePath filePath = default;
        DirectoryPath directoryPath = default;
        var fileException = await Assert.That(() => filePath.ThrowIfNull()).Throws<ArgumentNullException>();
        var directoryException = await Assert.That(() => directoryPath.ThrowIfNullOrWhiteSpace()).Throws<ArgumentNullException>();
        await Assert.That(fileException!.ParamName).IsEqualTo("filePath");
        await Assert.That(directoryException!.ParamName).IsEqualTo("directoryPath");
    }

    /// <summary>
    /// Verifies that blank path text is distinct from a null path reference.
    /// </summary>
    [Test]
    [Arguments("")]
    [Arguments(" \t ")]
    public async Task Should_reject_blank_path_text(string text)
    {
        var filePath = new FilePath(text);
        var directoryPath = new DirectoryPath(text);
        filePath.ThrowIfNull();
        directoryPath.ThrowIfNull();
        var fileException = await Assert.That(() => filePath.ThrowIfNullOrWhiteSpace()).Throws<ArgumentException>();
        var directoryException = await Assert.That(() => directoryPath.ThrowIfNotExists()).Throws<ArgumentException>();
        await Assert.That(fileException!.ParamName).IsEqualTo("filePath");
        await Assert.That(directoryException!.ParamName).IsEqualTo("directoryPath");
    }

    /// <summary>
    /// Verifies that nullable paths reject both missing values and default stored path text.
    /// </summary>
    [Test]
    public async Task Should_reject_null_and_default_nullable_paths()
    {
        FilePath? filePath = null;
        DirectoryPath? directoryPath = default(DirectoryPath);
        await Assert.That(() => filePath.ThrowIfNullOrWhiteSpace()).Throws<ArgumentNullException>();
        await Assert.That(() => directoryPath.ThrowIfNull()).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that checks do not normalize or query nonblank stored path text.
    /// </summary>
    [Test]
    public async Task Should_preserve_relative_path_text_after_success()
    {
        FilePath? filePath = new FilePath("relative/input.json");
        DirectoryPath? directoryPath = new DirectoryPath("relative");
        filePath.ThrowIfNullOrWhiteSpace();
        directoryPath.ThrowIfNullOrWhiteSpace();
        await Assert.That(filePath.Value.FullName).IsEqualTo("relative/input.json");
        await Assert.That(directoryPath.Value.FullName).IsEqualTo("relative");
    }
}