// -----------------------------------------------------------------------
// <copyright file="ThrowIfNullOrHasNoItemsTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class ThrowIfNullOrHasNoItemsTests
{
    /// <summary>
    /// Verifies that null file and folder collections report the caller's names.
    /// </summary>
    [Test]
    public async Task Should_report_caller_names_when_collections_are_null()
    {
        IReadOnlyCollection<FilePath>? files = null;
        IReadOnlyCollection<DirectoryPath>? folders = null;
        var fileException = await Assert.That(() => files.ThrowIfNullOrHasNoItems()).Throws<ArgumentNullException>();
        var folderException = await Assert.That(() => folders.ThrowIfNullOrHasNoItems()).Throws<ArgumentNullException>();
        await Assert.That(fileException!.ParamName).IsEqualTo("files");
        await Assert.That(folderException!.ParamName).IsEqualTo("folders");
    }

    /// <summary>
    /// Verifies that empty file and folder collections use argument exceptions.
    /// </summary>
    [Test]
    public async Task Should_reject_collections_when_count_is_zero()
    {
        FilePath[] files = [];
        DirectoryPath[] folders = [];
        var fileException = await Assert.That(() => files.ThrowIfNullOrHasNoItems()).Throws<ArgumentException>();
        var folderException = await Assert.That(() => folders.ThrowIfNullOrHasNoItems()).Throws<ArgumentException>();
        await Assert.That(fileException!.ParamName).IsEqualTo("files");
        await Assert.That(folderException!.ParamName).IsEqualTo("folders");
    }

    /// <summary>
    /// Verifies that collection checks use Count without enumerating or requiring existing paths.
    /// </summary>
    [Test]
    public async Task Should_accept_nonempty_collections_without_enumerating()
    {
        IReadOnlyCollection<FilePath>? files = new CountOnlyCollection<FilePath>();
        IReadOnlyCollection<DirectoryPath>? folders = new CountOnlyCollection<DirectoryPath>();
        files.ThrowIfNullOrHasNoItems();
        folders.ThrowIfNullOrHasNoItems();
        await Assert.That(files.Count).IsEqualTo(1);
        await Assert.That(folders.Count).IsEqualTo(1);
    }

    private sealed class CountOnlyCollection<T> : IReadOnlyCollection<T>
    {
        public int Count
        {
            get
            {
                return 1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new InvalidOperationException("Must not enumerate.");
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}