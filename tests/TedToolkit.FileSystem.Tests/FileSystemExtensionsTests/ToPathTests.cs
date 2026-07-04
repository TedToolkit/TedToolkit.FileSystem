// -----------------------------------------------------------------------
// <copyright file="ToPathTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemExtensionsTests;

internal sealed class ToPathTests
{
    /// <summary>
    /// Verifies that a directory info instance can be converted to a directory path through the extension method.
    /// </summary>
    [Test]
    public async Task Should_convert_directory_info_to_directory_path()
    {
        var path = TestAssets.NestedDirectory.ToPath();

        await Assert.That(path.FullName).IsEqualTo(TestAssets.NestedDirectory.FullName);
    }

    /// <summary>
    /// Verifies that a file info instance can be converted to a file path through the extension method.
    /// </summary>
    [Test]
    public async Task Should_convert_file_info_to_file_path()
    {
        var path = TestAssets.NestedFile.ToPath();

        await Assert.That(path.FullName).IsEqualTo(TestAssets.NestedFile.FullName);
    }

    /// <summary>
    /// Verifies that converting a null directory info instance preserves the public argument contract.
    /// </summary>
    [Test]
    public async Task Should_throw_argument_null_exception_when_converting_null_directory_info()
    {
        DirectoryInfo directoryInfo = null!;

        await Assert.That(() => directoryInfo.ToPath())
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that converting a null file info instance preserves the public argument contract.
    /// </summary>
    [Test]
    public async Task Should_throw_argument_null_exception_when_converting_null_file_info()
    {
        FileInfo fileInfo = null!;

        await Assert.That(() => fileInfo.ToPath())
            .Throws<ArgumentNullException>();
    }
}