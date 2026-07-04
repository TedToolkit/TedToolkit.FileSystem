// -----------------------------------------------------------------------
// <copyright file="AsFileNameTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem.Tests.FileSystemExtensionsTests;

internal sealed class AsFileNameTests
{
    /// <summary>
    /// Verifies that a string can be converted directly to a file name value object.
    /// </summary>
    [Test]
    public async Task Should_wrap_string_as_file_name()
    {
        var fileName = "report.txt".AsFileName();

        await Assert.That(fileName).IsEqualTo(new FileName("report.txt"));
    }
}
