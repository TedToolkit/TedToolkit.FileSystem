// -----------------------------------------------------------------------
// <copyright file="AdditionalFileApiTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text;

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class AdditionalFileApiTests
{
    /// <summary>
    /// 验证文件路径可以转接逐行读取接口。
    /// </summary>
    [Test]
    public async Task Should_read_lines_lazily_when_read_lines_helpers_are_used()
    {
        var file = TestWorkspace.CreateFile("lines.txt", "alpha" + Environment.NewLine + "beta");
        var path = new FilePath(file.FullName);

        try
        {
            await Assert.That(path.ReadLines().ToArray()).IsEquivalentTo(new[] { "alpha", "beta" });
            await Assert.That(path.ReadLines(Encoding.UTF8).ToArray()).IsEquivalentTo(new[] { "alpha", "beta" });

            var asyncLines = new List<string>();
            await foreach (var line in path.ReadLinesAsync(CancellationToken.None))
            {
                asyncLines.Add(line);
            }

            await Assert.That(asyncLines.ToArray()).IsEquivalentTo(new[] { "alpha", "beta" });
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    /// <summary>
    /// 验证文件路径可以转接额外的创建与打开重载。
    /// </summary>
    [Test]
    public async Task Should_open_and_create_file_when_additional_stream_overloads_are_used()
    {
        var file = TestWorkspace.CreateFile("stream-options.txt");
        var path = new FilePath(file.FullName);

        try
        {
            using (path.Create(256))
            {
            }

            using (path.Create(256, FileOptions.Asynchronous))
            {
            }

            using (path.Open(new FileStreamOptions
                   {
                       Mode = FileMode.Open,
                       Access = FileAccess.Read,
                       Share = FileShare.Read,
                   }))
            {
            }

            using (var handle = path.OpenHandle(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await Assert.That(handle.IsInvalid).IsFalse();
            }
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    /// <summary>
    /// 验证文件路径可以转接覆盖移动与字节追加接口。
    /// </summary>
    [Test]
#if NET6_0_OR_GREATER
    public async Task Should_support_additional_file_operation_overloads_when_requested()
    {
        var root = TestWorkspace.CreateDirectory();
        var source = new FilePath(Path.Combine(root.FullName, "source.txt"));
        var destination = new FilePath(Path.Combine(root.FullName, "destination.txt"));
        var appendBytesTarget = new FilePath(Path.Combine(root.FullName, "append-bytes.txt"));

        try
        {
            source.WriteAllText("new");
            destination.WriteAllText("old");

            source.MoveTo(destination, overwrite: true);
            await Assert.That(destination.ReadAllText()).IsEqualTo("new");

            appendBytesTarget.WriteAllBytes("ab"u8.ToArray());
            appendBytesTarget.AppendAllBytes("cd"u8.ToArray());
            await appendBytesTarget.AppendAllBytesAsync("ef"u8.ToArray(), CancellationToken.None);

            await Assert.That(appendBytesTarget.ReadAllText()).IsEqualTo("abcdef");
        }
        finally
        {
            root.Delete(true);
        }
    }
#endif

    /// <summary>
    /// 验证文件路径可以转接 span 与 memory 版本的文本和字节接口。
    /// </summary>
    [Test]
    public async Task Should_support_span_and_memory_content_overloads_when_requested()
    {
        var file = TestWorkspace.CreateFile("memory.txt");
        var path = new FilePath(file.FullName);

        try
        {
            path.WriteAllText("alpha".AsSpan());
            path.AppendAllText("beta".AsSpan());
            await Assert.That(path.ReadAllText()).IsEqualTo("alphabeta");

            await path.WriteAllTextAsync("gamma".AsMemory(), CancellationToken.None);
            await path.AppendAllTextAsync("delta".AsMemory(), CancellationToken.None);
            await Assert.That(await path.ReadAllTextAsync(CancellationToken.None)).IsEqualTo("gammadelta");

            path.WriteAllBytes("12"u8);
            await path.WriteAllBytesAsync("34"u8.ToArray().AsMemory(), CancellationToken.None);

            await Assert.That(await path.ReadAllTextAsync(CancellationToken.None)).IsEqualTo("34");
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }
}
