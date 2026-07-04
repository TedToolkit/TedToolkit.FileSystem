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
    /// Verifies that line-based read helpers return the expected file content.
    /// </summary>
    [Test]
    public async Task Should_read_lines_lazily_when_read_lines_helpers_are_used()
    {
        var file = TestWorkspace.CreateFile("lines.txt", "alpha" + Environment.NewLine + "beta");
        var path = new FilePath(file.FullName);

        try
        {
            await Assert.That(path.ReadLines().ToArray()).IsEquivalentTo(["alpha", "beta"]);
            await Assert.That(path.ReadLines(Encoding.UTF8).ToArray()).IsEquivalentTo(["alpha", "beta"]);

            var asyncLines = new List<string>();
            await foreach (var line in path.ReadLinesAsync(CancellationToken.None))
            {
                asyncLines.Add(line);
            }

            await Assert.That(asyncLines.ToArray()).IsEquivalentTo(["alpha", "beta"]);
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    /// <summary>
    /// Verifies that stream factory helpers cover all public create and open overloads.
    /// </summary>
    [Test]
    public async Task Should_open_and_create_file_when_additional_stream_overloads_are_used()
    {
        var file = TestWorkspace.CreateFile("stream-options.txt");
        var path = new FilePath(file.FullName);

        try
        {
            using (path.Create())
            {
            }

            using (path.Create(256))
            {
            }

            using (path.Create(256, FileOptions.Asynchronous))
            {
            }

            using (path.Open(FileMode.Open))
            {
            }

            using (path.Open(FileMode.Open, FileAccess.Read))
            {
            }

            using (path.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
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
    /// Verifies that file operation overloads cover overwrite and replacement paths.
    /// </summary>
    [Test]
#if NET6_0_OR_GREATER
    public async Task Should_support_additional_file_operation_overloads_when_requested()
    {
        var root = TestWorkspace.CreateDirectory();
        var source = new FilePath(Path.Combine(root.FullName, "source.txt"));
        var destination = new FilePath(Path.Combine(root.FullName, "destination.txt"));
        var appendBytesTarget = new FilePath(Path.Combine(root.FullName, "append-bytes.txt"));
        var replacement = new FilePath(Path.Combine(root.FullName, "replacement.txt"));
        var backup = new FilePath(Path.Combine(root.FullName, "backup.txt"));

        try
        {
            source.WriteAllText("new");
            destination.WriteAllText("old");

            source.CopyTo(destination, overwrite: true);
            await Assert.That(destination.ReadAllText()).IsEqualTo("new");

            source.WriteAllText("newer");
            source.MoveTo(destination, overwrite: true);
            await Assert.That(destination.ReadAllText()).IsEqualTo("newer");

            replacement.WriteAllText("replacement");
            replacement.Replace(destination, backup, ignoreMetadataErrors: true);

            await Assert.That(destination.ReadAllText()).IsEqualTo("replacement");
            await Assert.That(backup.ReadAllText()).IsEqualTo("newer");

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
    /// Verifies that sync text, line, and byte overloads cover the remaining public content APIs.
    /// </summary>
    [Test]
    public async Task Should_support_remaining_sync_content_overloads()
    {
        var file = TestWorkspace.CreateFile("sync-overloads.txt");
        var path = new FilePath(file.FullName);
        var expectedLines = new[] { "alpha", "beta", "gamma", "delta", };

        try
        {
            path.WriteAllText("alpha", Encoding.UTF8);
            await Assert.That(path.ReadAllText(Encoding.UTF8)).IsEqualTo("alpha");

            path.WriteAllText("beta".AsSpan());
            path.AppendAllText("gamma", Encoding.UTF8);
            path.AppendAllText("delta".AsSpan());
            path.AppendAllText("epsilon".AsSpan(), Encoding.UTF8);
            path.WriteAllText("zeta".AsSpan(), Encoding.UTF8);
            await Assert.That(path.ReadAllText()).IsEqualTo("zeta");

            path.WriteAllLines(expectedLines.Take(2));
            await Assert.That(path.ReadAllLines()).IsEquivalentTo(expectedLines.Take(2).ToArray());

            path.AppendAllLines(expectedLines.Skip(2).Take(1));
            path.AppendAllLines(expectedLines.Skip(3), Encoding.UTF8);
            await Assert.That(path.ReadAllLines(Encoding.UTF8)).IsEquivalentTo(expectedLines);

            path.WriteAllBytes("12"u8.ToArray());
            path.AppendAllBytes("34"u8.ToArray());
            path.WriteAllBytes("56"u8);
            path.AppendAllBytes("78"u8);
            await Assert.That(path.ReadAllBytes()).IsEquivalentTo("5678"u8.ToArray());
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    /// <summary>
    /// Verifies that async and memory-based content overloads cover the remaining public async APIs.
    /// </summary>
    [Test]
    public async Task Should_support_remaining_async_content_overloads()
    {
        var file = TestWorkspace.CreateFile("async-overloads.txt");
        var path = new FilePath(file.FullName);
        var expectedLines = new[] { "one", "two", "three", "four", };

        try
        {
            await path.WriteAllTextAsync("alpha", Encoding.UTF8, CancellationToken.None);
            await Assert.That(await path.ReadAllTextAsync(Encoding.UTF8, CancellationToken.None)).IsEqualTo("alpha");

            await path.WriteAllTextAsync("beta".AsMemory(), CancellationToken.None);
            await path.AppendAllTextAsync("gamma", CancellationToken.None);
            await path.AppendAllTextAsync("delta".AsMemory(), CancellationToken.None);
            await path.AppendAllTextAsync("epsilon".AsMemory(), Encoding.UTF8, CancellationToken.None);
            await path.WriteAllTextAsync("zeta".AsMemory(), Encoding.UTF8, CancellationToken.None);
            await Assert.That(await path.ReadAllTextAsync(CancellationToken.None)).IsEqualTo("zeta");

            await path.WriteAllLinesAsync(expectedLines.Take(2), CancellationToken.None);
            await Assert.That(await path.ReadAllLinesAsync(CancellationToken.None)).IsEquivalentTo(expectedLines.Take(2).ToArray());

            await path.AppendAllLinesAsync(expectedLines.Skip(2).Take(1), CancellationToken.None);
            await path.AppendAllLinesAsync(expectedLines.Skip(3), Encoding.UTF8, CancellationToken.None);
            await Assert.That(await path.ReadAllLinesAsync(Encoding.UTF8, CancellationToken.None)).IsEquivalentTo(expectedLines);

            var asyncLines = new List<string>();
            await foreach (var line in path.ReadLinesAsync(Encoding.UTF8, CancellationToken.None))
            {
                asyncLines.Add(line);
            }

            await Assert.That(asyncLines.ToArray()).IsEquivalentTo(expectedLines);

            await path.WriteAllBytesAsync("12"u8.ToArray(), CancellationToken.None);
            await path.WriteAllBytesAsync("34"u8.ToArray().AsMemory(), CancellationToken.None);
            await path.AppendAllBytesAsync("56"u8.ToArray().AsMemory(), CancellationToken.None);
            await Assert.That(await path.ReadAllBytesAsync(CancellationToken.None)).IsEquivalentTo("3456"u8.ToArray());
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }
}