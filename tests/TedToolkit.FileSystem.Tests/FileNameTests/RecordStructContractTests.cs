// -----------------------------------------------------------------------
// <copyright file="RecordStructContractTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileNameTests;

internal sealed class RecordStructContractTests
{
    /// <summary>
    /// Verifies that the file name value object preserves the provided name text.
    /// </summary>
    [Test]
    [Arguments("")]
    [Arguments("file.txt")]
    [Arguments("archive.tar.gz")]
    [Arguments("README")]
    public async Task Should_expose_input_text_through_name_when_constructed(string name)
    {
        var fileName = new FileName(name);

        await Assert.That(fileName.Name).IsEqualTo(name);
    }

    /// <summary>
    /// Verifies that two file name value objects are considered equal when their name text matches.
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_name_is_the_same()
    {
        var left = new FileName("file.txt");
        var right = new FileName("file.txt");

        await Assert.That(left == right).IsTrue();
    }

    /// <summary>
    /// Verifies that two file name value objects are considered not equal when their name text differs.
    /// </summary>
    [Test]
    public async Task Should_not_be_equal_when_name_is_different()
    {
        var left = new FileName("file.txt");
        var right = new FileName("other.txt");

        await Assert.That(left != right).IsTrue();
    }

    /// <summary>
    /// Verifies that the file name value object combines a base name and extension using BCL semantics.
    /// </summary>
    [Test]
    [Arguments("file", "txt")]
    [Arguments("file", ".txt")]
    [Arguments("archive.tar", ".gz")]
    [Arguments("README", null)]
    [Arguments("README", "")]
    public async Task Should_combine_name_and_extension_using_change_extension_semantics(string name, string? extension)
    {
        var fileName = new FileName(name, extension);

        await Assert.That(fileName.Name).IsEqualTo(Path.ChangeExtension(name, extension));
    }
}