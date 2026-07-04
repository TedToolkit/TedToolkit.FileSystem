// -----------------------------------------------------------------------
// <copyright file="RecordStructContractTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class RecordStructContractTests
{
    /// <summary>
    /// Verifies that the file path value object keeps the original full path text.
    /// </summary>
    [Test]
    public async Task Should_expose_input_text_through_full_name_when_constructed()
    {
        var fullName = TestAssets.NestedFile.FullName;
        var path = new FilePath(fullName);

        await Assert.That(path.FullName).IsEqualTo(fullName);
    }

    /// <summary>
    /// Verifies that two file path value objects are equal when the full path text matches.
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_full_name_is_the_same()
    {
        var fullName = TestAssets.RootFile.FullName;
        var left = new FilePath(fullName);
        var right = new FilePath(fullName);

        await Assert.That(left == right).IsTrue();
    }

    /// <summary>
    /// Verifies that two file path value objects are not equal when the full path text differs.
    /// </summary>
    [Test]
    public async Task Should_not_be_equal_when_full_name_is_different()
    {
        var left = new FilePath(TestAssets.NestedFile.FullName);
        var right = new FilePath(TestAssets.SiblingFile.FullName);

        await Assert.That(left != right).IsTrue();
    }
}
