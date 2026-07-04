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
    /// 验证创建文件路径值对象时会保留传入的完整路径文本。
    /// </summary>
    [Test]
    [Arguments("")]
    [Arguments("file.txt")]
    [Arguments("folder\\file.txt")]
    [Arguments("C:\\workspace\\file.txt")]
    public async Task Should_expose_input_text_through_full_name_when_constructed(string fullName)
    {
        var path = new FilePath(fullName);

        await Assert.That(path.FullName).IsEqualTo(fullName);
    }

    /// <summary>
    /// 验证两个文件路径值对象在完整路径文本相同时会被视为相等。
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_full_name_is_the_same()
    {
        var left = new FilePath("folder\\file.txt");
        var right = new FilePath("folder\\file.txt");

        await Assert.That(left == right).IsTrue();
    }

    /// <summary>
    /// 验证两个文件路径值对象在完整路径文本不同时会被视为不相等。
    /// </summary>
    [Test]
    public async Task Should_not_be_equal_when_full_name_is_different()
    {
        var left = new FilePath("folder\\file.txt");
        var right = new FilePath("folder\\other.txt");

        await Assert.That(left != right).IsTrue();
    }
}