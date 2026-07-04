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

    /// <summary>
    /// Verifies that a file info instance can be converted implicitly into a file path value object.
    /// </summary>
    [Test]
    public async Task Should_support_implicit_conversion_from_file_info()
    {
        FilePath path = TestAssets.NestedFile;

        await Assert.That(path.FullName).IsEqualTo(TestAssets.NestedFile.FullName);
    }

    /// <summary>
    /// Verifies that a file path can be created from a file info instance through the named factory method.
    /// </summary>
    [Test]
    public async Task Should_create_file_path_from_file_info()
    {
        var path = FilePath.FromFileInfo(TestAssets.NestedFile);

        await Assert.That(path.FullName).IsEqualTo(TestAssets.NestedFile.FullName);
    }

    /// <summary>
    /// Verifies that creating a file path from a null file info instance preserves the public argument contract.
    /// </summary>
    [Test]
    public async Task Should_throw_argument_null_exception_when_creating_file_path_from_null_file_info()
    {
        FileInfo fileInfo = null!;

        await Assert.That(() => FilePath.FromFileInfo(fileInfo))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that implicit conversion from a null file info instance matches the factory method contract.
    /// </summary>
    [Test]
    public async Task Should_throw_argument_null_exception_when_implicitly_converting_null_file_info()
    {
        FileInfo fileInfo = null!;

        await Assert.That(() => (FilePath)fileInfo)
            .Throws<ArgumentNullException>();
    }
}