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
    /// 验证创建文件名值对象时会保留传入的名称文本。
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
    /// 验证两个文件名值对象在名称文本相同时会被视为相等。
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_name_is_the_same()
    {
        var left = new FileName("file.txt");
        var right = new FileName("file.txt");

        await Assert.That(left == right).IsTrue();
    }

    /// <summary>
    /// 验证两个文件名值对象在名称文本不同时会被视为不相等。
    /// </summary>
    [Test]
    public async Task Should_not_be_equal_when_name_is_different()
    {
        var left = new FileName("file.txt");
        var right = new FileName("other.txt");

        await Assert.That(left != right).IsTrue();
    }
}