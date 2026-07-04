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
    /// Verifies that parent-directory helpers create missing parent directories before writing.
    /// </summary>
    [Test]
    public async Task Should_create_parent_directory_when_requested()
    {
        var root = TestWorkspace.CreateDirectory();
        var path = new FilePath(Path.Combine(root.FullName, "nested", "child.txt"));

        try
        {
            var directory = path.CreateParentDirectory();

            await Assert.That(directory.FullName).IsEqualTo(Path.Combine(root.FullName, "nested"));
            await Assert.That(Directory.Exists(directory.FullName)).IsTrue();
        }
        finally
        {
            root.Delete(true);
        }
    }

    /// <summary>
    /// Verifies that text, bytes, lines, and async helpers read back the written file contents.
    /// </summary>
    [Test]
    public async Task Should_round_trip_file_contents_when_using_sync_and_async_helpers()
    {
        var file = TestWorkspace.CreateFile("content.txt");
        var path = new FilePath(file.FullName);
        var lines = new[] { "alpha", "beta" };

        try
        {
            path.WriteAllText("hello");
            await Assert.That(path.ReadAllText()).IsEqualTo("hello");

            path.AppendAllText(" world");
            await Assert.That(path.ReadAllText()).IsEqualTo("hello world");

            path.WriteAllLines(lines, Encoding.UTF8);
            await Assert.That(path.ReadAllLines(Encoding.UTF8)).IsEquivalentTo(lines);

            path.WriteAllBytes("abc"u8.ToArray());
            await Assert.That(path.ReadAllBytes()).IsEquivalentTo("abc"u8.ToArray());

            await path.WriteAllTextAsync("async", CancellationToken.None);
            await Assert.That(await path.ReadAllTextAsync(CancellationToken.None)).IsEqualTo("async");

            await path.AppendAllTextAsync(" text", Encoding.UTF8, CancellationToken.None);
            await Assert.That(await path.ReadAllTextAsync(Encoding.UTF8, CancellationToken.None)).IsEqualTo("async text");

            await path.WriteAllLinesAsync(lines, Encoding.UTF8, CancellationToken.None);
            await Assert.That(await path.ReadAllLinesAsync(Encoding.UTF8, CancellationToken.None)).IsEquivalentTo(lines);

            await path.WriteAllBytesAsync("xyz"u8.ToArray(), CancellationToken.None);
            await Assert.That(await path.ReadAllBytesAsync(CancellationToken.None)).IsEquivalentTo("xyz"u8.ToArray());
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

            using (var writer = path.CreateText())
            {
                await writer.WriteAsync("created");
            }

            using (var writer = path.AppendText())
            {
                await writer.WriteAsync("+append");
            }

            await Assert.That(path.ReadAllText()).IsEqualTo("created+append");
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
            source.WriteAllText("source");
            replacement.WriteAllText("replacement");

            source.CopyTo(copy);
            await Assert.That(copy.ReadAllText()).IsEqualTo("source");

            copy.MoveTo(moved);
            await Assert.That(moved.Exists).IsTrue();
            await Assert.That(copy.Exists).IsFalse();

            replacement.Replace(moved, backup);

            await Assert.That(moved.ReadAllText()).IsEqualTo("replacement");
            await Assert.That(backup.ReadAllText()).IsEqualTo("source");

            moved.Delete();
            await Assert.That(moved.Exists).IsFalse();
        }
        finally
        {
            root.Delete(true);
        }
    }
}
