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
            await Assert.That(ReadLines(path).ToArray())
                .IsEquivalentTo([
                    "alpha",
                    "beta",
                ]);
            await Assert.That(ReadLines(path, Encoding.UTF8).ToArray())
                .IsEquivalentTo([
                    "alpha",
                    "beta",
                ]);

            var asyncLines = new List<string>();
            await foreach (var line in path.ReadLinesAsync(CancellationToken.None).ConfigureAwait(false))
            {
                asyncLines.Add(line);
            }

            await Assert.That(asyncLines.ToArray())
                .IsEquivalentTo([
                    "alpha",
                    "beta",
                ]);
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

            using (path.Open(new FileStreamOptions()
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
    /// Verifies that sync overloads remain usable without composing custom logic.
    /// </summary>
    [Test]
    public async Task Should_support_remaining_sync_content_overloads()
    {
        var file = TestWorkspace.CreateFile("sync-overloads.txt");
        var path = new FilePath(file.FullName);
        var expectedLines = new[] { "alpha", "beta", "gamma", "delta", };

        try
        {
            var result = ExerciseSyncOverloads(in path, expectedLines);

            await Assert.That(result.InitialText).IsEqualTo("alpha");
            await Assert.That(result.FinalText).IsEqualTo("zeta");
            await Assert.That(result.LinesAfterWrite).IsEquivalentTo(expectedLines.Take(2).ToArray());
            await Assert.That(result.LinesAfterAppend).IsEquivalentTo(expectedLines);
            await Assert.That(result.FinalBytes).IsEquivalentTo("5678"u8.ToArray());
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
            var result = ExerciseAdditionalFileOperations(
                in source,
                in destination,
                in appendBytesTarget,
                in replacement,
                in backup);

            await Assert.That(result.DestinationText).IsEqualTo("replacement");
            await Assert.That(result.BackupText).IsEqualTo("newer");
            await Assert.That(result.AppendedBytesText).IsEqualTo("abcdef");
        }
        finally
        {
            root.Delete(true);
        }
    }
#endif

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
            var alpha = await path.ReadAllTextAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await Assert.That(alpha).IsEqualTo("alpha");

            await path.WriteAllTextAsync("beta".AsMemory(), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllTextAsync("gamma", CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllTextAsync("delta".AsMemory(), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllTextAsync("epsilon".AsMemory(), Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await path.WriteAllTextAsync("zeta".AsMemory(), Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            var zeta = await path.ReadAllTextAsync(CancellationToken.None).ConfigureAwait(false);
            await Assert.That(zeta).IsEqualTo("zeta");

            await path.WriteAllLinesAsync(expectedLines.Take(2), CancellationToken.None).ConfigureAwait(false);
            var firstExpectedLines = await path.ReadAllLinesAsync(CancellationToken.None).ConfigureAwait(false);
            await Assert.That(firstExpectedLines).IsEquivalentTo(expectedLines.Take(2).ToArray());

            await path.AppendAllLinesAsync(expectedLines.Skip(2).Take(1), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllLinesAsync(expectedLines.Skip(3), Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            var allExpectedLines = await path.ReadAllLinesAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false);
            await Assert.That(allExpectedLines).IsEquivalentTo(expectedLines);

            var asyncLines = new List<string>();
            await foreach (var line in path.ReadLinesAsync(Encoding.UTF8, CancellationToken.None).ConfigureAwait(false))
            {
                asyncLines.Add(line);
            }

            await Assert.That(asyncLines.ToArray()).IsEquivalentTo(expectedLines);

            await path.WriteAllBytesAsync("12"u8.ToArray(), CancellationToken.None).ConfigureAwait(false);
            await path.WriteAllBytesAsync("34"u8.ToArray().AsMemory(), CancellationToken.None).ConfigureAwait(false);
            await path.AppendAllBytesAsync("56"u8.ToArray().AsMemory(), CancellationToken.None).ConfigureAwait(false);
            var bytes = await path.ReadAllBytesAsync(CancellationToken.None).ConfigureAwait(false);
            await Assert.That(bytes).IsEquivalentTo("3456"u8.ToArray());
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    private static (
        string InitialText,
        string FinalText,
        string[] LinesAfterWrite,
        string[] LinesAfterAppend,
        byte[] FinalBytes) ExerciseSyncOverloads(
        in FilePath path,
        IReadOnlyList<string> expectedLines)
    {
        path.WriteAllText("alpha", Encoding.UTF8);
        var initialText = path.ReadAllText(Encoding.UTF8);

        path.WriteAllText("beta".AsSpan());
        path.AppendAllText("gamma", Encoding.UTF8);
        path.AppendAllText("delta".AsSpan());
        path.AppendAllText("epsilon".AsSpan(), Encoding.UTF8);
        path.WriteAllText("zeta".AsSpan(), Encoding.UTF8);
        var finalText = path.ReadAllText();

        path.WriteAllLines(expectedLines.Take(2));
        var linesAfterWrite = path.ReadAllLines();

        path.AppendAllLines(expectedLines.Skip(2).Take(1));
        path.AppendAllLines(expectedLines.Skip(3), Encoding.UTF8);
        var linesAfterAppend = path.ReadAllLines(Encoding.UTF8);

        path.WriteAllBytes("12"u8.ToArray());
        path.AppendAllBytes("34"u8.ToArray());
        path.WriteAllBytes("56"u8);
        path.AppendAllBytes("78"u8);
        var finalBytes = path.ReadAllBytes();

        return (initialText, finalText, linesAfterWrite, linesAfterAppend, finalBytes);
    }

#if NET6_0_OR_GREATER
    private static (
        string DestinationText,
        string BackupText,
        string AppendedBytesText) ExerciseAdditionalFileOperations(
        in FilePath source,
        in FilePath destination,
        in FilePath appendBytesTarget,
        in FilePath replacement,
        in FilePath backup)
    {
        source.WriteAllText("new");
        destination.WriteAllText("old");

        source.CopyTo(destination, overwrite: true);
        source.WriteAllText("newer");
        source.MoveTo(destination, overwrite: true);

        replacement.WriteAllText("replacement");
        replacement.Replace(destination, backup, ignoreMetadataErrors: true);

        appendBytesTarget.WriteAllBytes("ab"u8.ToArray());
        appendBytesTarget.AppendAllBytes("cd"u8.ToArray());
        appendBytesTarget.AppendAllBytes("ef"u8.ToArray());

        return (
            destination.ReadAllText(),
            backup.ReadAllText(),
            appendBytesTarget.ReadAllText());
    }
#endif

    private static IEnumerable<string> ReadLines(in FilePath path)
    {
        return path.ReadLines();
    }

    private static IEnumerable<string> ReadLines(in FilePath path, Encoding encoding)
    {
        return path.ReadLines(encoding);
    }
}