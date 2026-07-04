// -----------------------------------------------------------------------
// <copyright file="StaticDirectoryTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.DirectoryPathTests;

internal sealed class StaticDirectoryTests
{
    [Test]
    public async Task Should_expose_current_directory_through_static_property()
    {
        var originalCurrentDirectory = DirectoryPath.CurrentDirectory;
        var temporaryDirectory = TestWorkspace.CreateDirectory();
        var expectedPath = new DirectoryPath(temporaryDirectory.FullName);

        try
        {
            DirectoryPath.CurrentDirectory = expectedPath;

            await Assert.That(DirectoryPath.CurrentDirectory).IsEqualTo(expectedPath);
        }
        finally
        {
            DirectoryPath.CurrentDirectory = originalCurrentDirectory;
            DeleteDirectoryIfExists(temporaryDirectory.FullName);
        }
    }

    [Test]
    public async Task Should_expose_logical_drives_through_static_property()
    {
        var currentDrive = new DirectoryPath(Path.GetPathRoot(Environment.CurrentDirectory)!);

        var logicalDrives = DirectoryPath.LogicalDrives;

        await Assert.That(logicalDrives).Contains(currentDrive);
    }

    [Test]
    public async Task Should_create_temporary_subdirectory_through_static_method()
    {
        var temporaryDirectory = DirectoryPath.CreateTempSubdirectory("ttfs-");

        try
        {
            await Assert.That(temporaryDirectory.Exists).IsTrue();
            await Assert.That(Path.GetFileName(temporaryDirectory.FullName)).StartsWith("ttfs-");
        }
        finally
        {
            DeleteDirectoryIfExists(temporaryDirectory.FullName);
        }
    }

    private static void DeleteDirectoryIfExists(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            File.SetAttributes(directoryPath, FileAttributes.Normal);
            Directory.Delete(directoryPath, true);
        }
    }
}
