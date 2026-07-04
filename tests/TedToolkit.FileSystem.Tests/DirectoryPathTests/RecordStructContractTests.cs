// -----------------------------------------------------------------------
// <copyright file="RecordStructContractTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.DirectoryPathTests;

internal sealed class RecordStructContractTests
{
    /// <summary>
    /// Verifies that the directory path value object keeps the original full path text.
    /// </summary>
    [Test]
    public async Task Should_expose_input_text_through_full_name_when_constructed()
    {
        var fullName = TestAssets.NestedDirectory.FullName;
        var path = new DirectoryPath(fullName);

        await Assert.That(path.FullName).IsEqualTo(fullName);
    }

    /// <summary>
    /// Verifies that two directory path value objects are equal when the full path text matches.
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_full_name_is_the_same()
    {
        var fullName = TestAssets.RootDirectory.FullName;
        var left = new DirectoryPath(fullName);
        var right = new DirectoryPath(fullName);

        await Assert.That(left == right).IsTrue();
    }

    /// <summary>
    /// Verifies that two directory path value objects are not equal when the full path text differs.
    /// </summary>
    [Test]
    public async Task Should_not_be_equal_when_full_name_is_different()
    {
        var left = new DirectoryPath(TestAssets.NestedDirectory.FullName);
        var right = new DirectoryPath(TestAssets.SiblingDirectory.FullName);

        await Assert.That(left != right).IsTrue();
    }

    /// <summary>
    /// Verifies that a directory info instance can be converted implicitly into a directory path value object.
    /// </summary>
    [Test]
    public async Task Should_support_implicit_conversion_from_directory_info()
    {
        DirectoryPath path = TestAssets.NestedDirectory;

        await Assert.That(path.FullName).IsEqualTo(TestAssets.NestedDirectory.FullName);
    }

    /// <summary>
    /// Verifies that a directory path can be created from a directory info instance through the named factory method.
    /// </summary>
    [Test]
    public async Task Should_create_directory_path_from_directory_info()
    {
        var path = DirectoryPath.FromDirectoryInfo(TestAssets.NestedDirectory);

        await Assert.That(path.FullName).IsEqualTo(TestAssets.NestedDirectory.FullName);
    }

    /// <summary>
    /// Verifies that combining a directory path with a string creates a child directory path.
    /// </summary>
    [Test]
    public async Task Should_create_child_directory_when_combined_with_string()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var childDirectory = path / "nested";

        await Assert.That(childDirectory.FullName).IsEqualTo(Path.Combine(TestAssets.RootDirectory.FullName, "nested"));
    }

    /// <summary>
    /// Verifies that combining a directory path with a string through the named method creates a child directory path.
    /// </summary>
    [Test]
    public async Task Should_create_child_directory_when_combined_with_string_method()
    {
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        var childDirectory = path.Combine("nested");

        await Assert.That(childDirectory.FullName).IsEqualTo(Path.Combine(TestAssets.RootDirectory.FullName, "nested"));
    }

    /// <summary>
    /// Verifies that combining a directory path with a file name creates a child file path.
    /// </summary>
    [Test]
    public async Task Should_create_child_file_when_combined_with_file_name()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        var childFile = path / new FileName("nested-file.txt");

        await Assert.That(childFile.FullName).IsEqualTo(Path.Combine(TestAssets.NestedDirectory.FullName, "nested-file.txt"));
    }

    /// <summary>
    /// Verifies that combining a directory path with a file name through the named method creates a child file path.
    /// </summary>
    [Test]
    public async Task Should_create_child_file_when_combined_with_file_name_method()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        var childFile = path.Combine(new FileName("nested-file.txt"));

        await Assert.That(childFile.FullName).IsEqualTo(Path.Combine(TestAssets.NestedDirectory.FullName, "nested-file.txt"));
    }
}