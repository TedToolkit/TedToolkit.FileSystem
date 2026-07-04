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
            await Assert.That(ReadLines(in path).ToArray()).IsEquivalentTo(["alpha", "beta"]);
            await Assert.That(ReadLines(in path, Encoding.UTF8).ToArray()).IsEquivalentTo(["alpha", "beta"]);

            var asyncLines = new List<string>();
            await foreach (var line in path.ReadLinesAsync(CancellationToken.None).ConfigureAwait(false))
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

            using (path.Open(new()
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
            WriteAllText(in source, "new");
            WriteAllText(in destination, "old");

            source.CopyTo(destination, overwrite: true);
            await Assert.That(ReadAllText(in destination)).IsEqualTo("new");

            WriteAllText(in source, "newer");
            source.MoveTo(destination, overwrite: true);
            await Assert.That(ReadAllText(in destination)).IsEqualTo("newer");

            WriteAllText(in replacement, "replacement");
            replacement.Replace(destination, backup, ignoreMetadataErrors: true);

            await Assert.That(ReadAllText(in destination)).IsEqualTo("replacement");
            await Assert.That(ReadAllText(in backup)).IsEqualTo("newer");

            WriteAllBytes(in appendBytesTarget, "ab"u8.ToArray());
            AppendAllBytes(in appendBytesTarget, "cd"u8.ToArray());
            await appendBytesTarget.AppendAllBytesAsync("ef"u8.ToArray(), CancellationToken.None).ConfigureAwait(false);

            await Assert.That(ReadAllText(in appendBytesTarget)).IsEqualTo("abcdef");
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
            WriteAllText(in path, "alpha", Encoding.UTF8);
            await Assert.That(ReadAllText(in path, Encoding.UTF8)).IsEqualTo("alpha");

            WriteAllText(in path, "beta".AsSpan());
            AppendAllText(in path, "gamma", Encoding.UTF8);
            AppendAllText(in path, "delta".AsSpan());
            AppendAllText(in path, "epsilon".AsSpan(), Encoding.UTF8);
            WriteAllText(in path, "zeta".AsSpan(), Encoding.UTF8);
            await Assert.That(ReadAllText(in path)).IsEqualTo("zeta");

            WriteAllLines(in path, expectedLines.Take(2));
            await Assert.That(ReadAllLines(in path)).IsEquivalentTo(expectedLines.Take(2).ToArray());

            AppendAllLines(in path, expectedLines.Skip(2).Take(1));
            AppendAllLines(in path, expectedLines.Skip(3), Encoding.UTF8);
            await Assert.That(ReadAllLines(in path, Encoding.UTF8)).IsEquivalentTo(expectedLines);

            WriteAllBytes(in path, "12"u8.ToArray());
            AppendAllBytes(in path, "34"u8.ToArray());
            WriteAllBytes(in path, "56"u8);
            AppendAllBytes(in path, "78"u8);
            await Assert.That(ReadAllBytes(in path)).IsEquivalentTo("5678"u8.ToArray());
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
            await path.WriteAllTextAsync("alpha", Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllTextAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false)).IsEqualTo("alpha");

            await path.WriteAllTextAsync("beta".AsMemory(), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllTextAsync("gamma", CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllTextAsync("delta".AsMemory(), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllTextAsync("epsilon".AsMemory(), Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await path.WriteAllTextAsync("zeta".AsMemory(), Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllTextAsync(CancellationToken.None).ConfigureAwait(false)).IsEqualTo("zeta");

            await path.WriteAllLinesAsync(expectedLines.Take(2), CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllLinesAsync(CancellationToken.None).ConfigureAwait(false)).IsEquivalentTo(expectedLines.Take(2).ToArray());

            await path.AppendAllLinesAsync(expectedLines.Skip(2).Take(1), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllLinesAsync(expectedLines.Skip(3), Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllLinesAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false)).IsEquivalentTo(expectedLines);

            var asyncLines = new List<string>();
            await foreach (var line in path.ReadLinesAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false))
            {
                asyncLines.Add(line);
            }

            await Assert.That(asyncLines.ToArray()).IsEquivalentTo(expectedLines);

            await path.WriteAllBytesAsync("12"u8.ToArray(), CancellationToken.None).ConfigureAwait(false);
            await path.WriteAllBytesAsync("34"u8.ToArray().AsMemory(), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllBytesAsync("56"u8.ToArray().AsMemory(), CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllBytesAsync(CancellationToken.None).ConfigureAwait(false)).IsEquivalentTo("3456"u8.ToArray());
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    private static void AppendAllBytes(in FilePath path, byte[] bytes)
    {
        path.AppendAllBytes(bytes);
    }

    private static void AppendAllBytes(in FilePath path, ReadOnlySpan<byte> bytes)
    {
        path.AppendAllBytes(bytes);
    }

    private static void AppendAllLines(in FilePath path, IEnumerable<string> contents)
    {
        path.AppendAllLines(contents);
    }

    private static void AppendAllLines(in FilePath path, IEnumerable<string> contents, Encoding encoding)
    {
        path.AppendAllLines(contents, encoding);
    }

    private static void AppendAllText(in FilePath path, string contents, Encoding encoding)
    {
        path.AppendAllText(contents, encoding);
    }

    private static void AppendAllText(in FilePath path, ReadOnlySpan<char> contents)
    {
        path.AppendAllText(contents);
    }

    private static void AppendAllText(in FilePath path, ReadOnlySpan<char> contents, Encoding encoding)
    {
        path.AppendAllText(contents, encoding);
    }

    private static byte[] ReadAllBytes(in FilePath path)
    {
        return path.ReadAllBytes();
    }

    private static string[] ReadAllLines(in FilePath path)
    {
        return path.ReadAllLines();
    }

    private static string[] ReadAllLines(in FilePath path, Encoding encoding)
    {
        return path.ReadAllLines(encoding);
    }

    private static string ReadAllText(in FilePath path)
    {
        return path.ReadAllText();
    }

    private static string ReadAllText(in FilePath path, Encoding encoding)
    {
        return path.ReadAllText(encoding);
    }

    private static IEnumerable<string> ReadLines(in FilePath path)
    {
        return path.ReadLines();
    }

    private static IEnumerable<string> ReadLines(in FilePath path, Encoding encoding)
    {
        return path.ReadLines(encoding);
    }

    private static void WriteAllBytes(in FilePath path, byte[] bytes)
    {
        path.WriteAllBytes(bytes);
    }

    private static void WriteAllBytes(in FilePath path, ReadOnlySpan<byte> bytes)
    {
        path.WriteAllBytes(bytes);
    }

    private static void WriteAllLines(in FilePath path, IEnumerable<string> contents)
    {
        path.WriteAllLines(contents);
    }

    private static void WriteAllText(in FilePath path, string contents)
    {
        path.WriteAllText(contents);
    }

    private static void WriteAllText(in FilePath path, string contents, Encoding encoding)
    {
        path.WriteAllText(contents, encoding);
    }

    private static void WriteAllText(in FilePath path, ReadOnlySpan<char> contents)
    {
        path.WriteAllText(contents);
    }

    private static void WriteAllText(in FilePath path, ReadOnlySpan<char> contents, Encoding encoding)
    {
        path.WriteAllText(contents, encoding);
    }
}
