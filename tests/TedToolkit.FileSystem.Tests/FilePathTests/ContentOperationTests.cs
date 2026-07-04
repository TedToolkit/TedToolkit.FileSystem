// -----------------------------------------------------------------------
// <copyright file="ContentOperationTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text;

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class ContentOperationTests
{
    /// <summary>
    /// Verifies that text, bytes, lines, and async helpers read back the written file contents.
    /// </summary>
    [Test]
    public async Task Should_round_trip_file_contents_when_using_sync_and_async_helpers()
    {
        var file = TestWorkspace.CreateFile("content.txt");
        var path = new FilePath(file.FullName);
        var lines = new[] { "alpha", "beta", };

        try
        {
            WriteAllText(in path, "hello");
            await Assert.That(ReadAllText(in path)).IsEqualTo("hello");

            AppendAllText(in path, " world");
            await Assert.That(ReadAllText(in path)).IsEqualTo("hello world");

            WriteAllLines(in path, lines, Encoding.UTF8);
            await Assert.That(ReadAllLines(in path, Encoding.UTF8)).IsEquivalentTo(lines);

            WriteAllBytes(in path, "abc"u8.ToArray());
            await Assert.That(ReadAllBytes(in path)).IsEquivalentTo("abc"u8.ToArray());

            await path.WriteAllTextAsync("async", CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllTextAsync(CancellationToken.None).ConfigureAwait(false)).IsEqualTo("async");

            await path.AppendAllTextAsync(" text", Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllTextAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false)).IsEqualTo("async text");

            await path.WriteAllLinesAsync(lines, Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllLinesAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false)).IsEquivalentTo(lines);

            await path.WriteAllBytesAsync("xyz"u8.ToArray(), CancellationToken.None).ConfigureAwait(false);
            await Assert.That(await path.ReadAllBytesAsync(CancellationToken.None).ConfigureAwait(false)).IsEquivalentTo("xyz"u8.ToArray());
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    /// <summary>
    /// Verifies that stream factory helpers open readable and writable streams for the file path.
    /// </summary>
    [Test]
    public async Task Should_open_streams_and_text_helpers_when_requested()
    {
        var file = TestWorkspace.CreateFile("stream.txt", "seed");
        var path = new FilePath(file.FullName);

        try
        {
            using (path.OpenWrite())
            {
            }

            using (path.OpenRead())
            {
            }

            using (path.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
            }

            using (path.OpenText())
            {
            }

            await using (var writer = path.CreateText())
            {
                await writer.WriteAsync("created".AsMemory()).ConfigureAwait(false);
            }

            await using (var writer = path.AppendText())
            {
                await writer.WriteAsync("+append".AsMemory()).ConfigureAwait(false);
            }

            await Assert.That(ReadAllText(in path)).IsEqualTo("created+append");
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    /// <summary>
    /// Verifies that copy, move, replace, and delete helpers delegate to file operations.
    /// </summary>
    [Test]
    public async Task Should_perform_file_lifecycle_operations_when_requested()
    {
        var root = TestWorkspace.CreateDirectory();
        var source = new FilePath(Path.Combine(root.FullName, "source.txt"));
        var copy = new FilePath(Path.Combine(root.FullName, "copy.txt"));
        var moved = new FilePath(Path.Combine(root.FullName, "moved.txt"));
        var replacement = new FilePath(Path.Combine(root.FullName, "replacement.txt"));
        var backup = new FilePath(Path.Combine(root.FullName, "backup.txt"));

        try
        {
            WriteAllText(in source, "source");
            WriteAllText(in replacement, "replacement");

            source.CopyTo(copy);
            await Assert.That(ReadAllText(in copy)).IsEqualTo("source");

            copy.MoveTo(moved);
            await Assert.That(moved.Exists).IsTrue();
            await Assert.That(copy.Exists).IsFalse();

            replacement.Replace(moved, backup);

            await Assert.That(ReadAllText(in moved)).IsEqualTo("replacement");
            await Assert.That(ReadAllText(in backup)).IsEqualTo("source");

            moved.Delete();
            await Assert.That(moved.Exists).IsFalse();
        }
        finally
        {
            root.Delete(true);
        }
    }

    private static void AppendAllText(in FilePath path, string contents)
    {
        path.AppendAllText(contents);
    }

    private static string[] ReadAllLines(in FilePath path, Encoding encoding)
    {
        return path.ReadAllLines(encoding);
    }

    private static byte[] ReadAllBytes(in FilePath path)
    {
        return path.ReadAllBytes();
    }

    private static string ReadAllText(in FilePath path)
    {
        return path.ReadAllText();
    }

    private static void WriteAllBytes(in FilePath path, byte[] bytes)
    {
        path.WriteAllBytes(bytes);
    }

    private static void WriteAllLines(in FilePath path, IEnumerable<string> contents, Encoding encoding)
    {
        path.WriteAllLines(contents, encoding);
    }

    private static void WriteAllText(in FilePath path, string contents)
    {
        path.WriteAllText(contents);
    }
}
