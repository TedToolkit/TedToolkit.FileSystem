// -----------------------------------------------------------------------
// <copyright file="ToFileNameTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem.Tests.FileSystemExtensionsTests;

internal sealed class ToFileNameTests
{
    /// <summary>
    /// Verifies that a string can be converted directly to a file name value object.
    /// </summary>
    [Test]
    public async Task Should_convert_string_to_file_name()
    {
        var fileName = "report.txt".ToFileName();

        await Assert.That(fileName).IsEqualTo(new FileName("report.txt"));
    }

    /// <summary>
    /// Verifies that a base file name can be combined with an extension.
    /// </summary>
    [Test]
    public async Task Should_convert_base_name_to_file_name_with_extension()
    {
        var fileName = "report".ToFileName("txt");

        await Assert.That(fileName).IsEqualTo(new FileName("report.txt"));
    }

    /// <summary>
    /// Verifies that an existing file name can be transformed to use a different extension.
    /// </summary>
    [Test]
    public async Task Should_convert_existing_file_name_to_file_name_with_replaced_extension()
    {
        var fileName = "report.log".ToFileNameWithExtension("txt");

        await Assert.That(fileName).IsEqualTo(new FileName("report.txt"));
    }
}
