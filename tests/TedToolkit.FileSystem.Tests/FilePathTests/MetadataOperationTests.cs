// -----------------------------------------------------------------------
// <copyright file="MetadataOperationTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class MetadataOperationTests
{
    /// <summary>
    /// Verifies that metadata setters preserve the behavior of the underlying file system.
    /// </summary>
    [Test]
    public async Task Should_update_file_metadata_when_properties_are_set()
    {
        var file = TestWorkspace.CreateFile("timestamps.txt", "content");
        var path = new FilePath(file.FullName);
        var creationTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Local);
        var lastWriteTime = new DateTime(2024, 2, 3, 4, 5, 6, DateTimeKind.Local);
        var lastAccessTime = new DateTime(2024, 3, 4, 5, 6, 7, DateTimeKind.Local);

        try
        {
            path.IsReadOnly = true;
            File.SetCreationTime(file.FullName, creationTime);
            var effectiveCreationTime = File.GetCreationTime(file.FullName);
            File.SetCreationTime(file.FullName, creationTime.AddDays(1));
            path.CreationTime = creationTime;
            await Assert.That(path.CreationTime).IsEqualTo(effectiveCreationTime);

            path.LastWriteTime = lastWriteTime;
            path.LastAccessTime = lastAccessTime;
            path.Attributes = FileAttributes.ReadOnly;

            await Assert.That(path.IsReadOnly).IsTrue();
            await Assert.That(path.LastWriteTime).IsEqualTo(lastWriteTime);
            await Assert.That(path.LastAccessTime).IsEqualTo(lastAccessTime);
            await Assert.That(path.Attributes).IsEqualTo(FileAttributes.ReadOnly);
        }
        finally
        {
            File.SetAttributes(file.FullName, FileAttributes.Normal);
            file.Directory!.Delete(true);
        }
    }
}