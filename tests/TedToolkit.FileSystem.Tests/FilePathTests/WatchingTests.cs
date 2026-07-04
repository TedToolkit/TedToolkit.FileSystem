// -----------------------------------------------------------------------
// <copyright file="WatchingTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class WatchingTests
{
    /// <summary>
    /// Verifies that file paths can create a file system watcher scoped to the current file.
    /// </summary>
    [Test]
    public async Task Should_create_file_system_watcher_for_current_file_when_watch_is_requested()
    {
        var file = TestWorkspace.CreateFile("watch.txt");
        var path = new FilePath(file.FullName);

        try
        {
            using var watcher = path.Watch();

            await Assert.That(watcher.Path).IsEqualTo(file.DirectoryName);
            await Assert.That(watcher.Filter).IsEqualTo(file.Name);
            await Assert.That(watcher.EnableRaisingEvents).IsTrue();
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }

    /// <summary>
    /// Verifies that file paths without a parent directory cannot create a watcher.
    /// </summary>
    [Test]
    public async Task Should_throw_when_watch_is_requested_without_parent_directory()
    {
        var path = new FilePath("watch.txt");

        await Assert.That(() => path.Watch())
            .Throws<ArgumentException>();
    }
}