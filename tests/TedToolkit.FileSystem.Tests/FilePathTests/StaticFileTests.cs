// -----------------------------------------------------------------------
// <copyright file="StaticFileTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class StaticFileTests
{
    [Test]
    public async Task Should_create_temporary_file_through_static_method()
    {
        var temporaryFile = FilePath.GetTempFileName();

        try
        {
            await Assert.That(temporaryFile.Exists).IsTrue();
            await Assert.That(Path.GetDirectoryName(temporaryFile.FullName))
                .IsEqualTo(Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }
        finally
        {
            DeleteFileIfExists(temporaryFile.FullName);
        }
    }

    [Test]
    public async Task Should_create_file_path_from_assembly()
    {
        var path = FilePath.FromAssembly(typeof(FilePath).Assembly);

        await Assert.That(path).IsEqualTo(new FilePath(typeof(FilePath).Assembly.Location));
    }

    [Test]
    public async Task Should_create_file_path_from_executing_assembly()
    {
        var path = FilePath.FromExecutingAssembly();

        await Assert.That(path).IsEqualTo(new FilePath(typeof(FilePath).Assembly.Location));
    }

    [Test]
    public async Task Should_create_file_path_from_calling_assembly()
    {
        var path = GetPathFromCallingAssembly();

        await Assert.That(path).IsEqualTo(new FilePath(typeof(StaticFileTests).Assembly.Location));
    }

    [Test]
    public async Task Should_create_file_path_from_entry_assembly_when_available()
    {
        var expectedAssembly = System.Reflection.Assembly.GetEntryAssembly();
        var path = FilePath.FromEntryAssembly();

        if (expectedAssembly is null)
        {
            await Assert.That(path).IsEqualTo(default(FilePath?));
            return;
        }

        await Assert.That(path).IsEqualTo(new FilePath(expectedAssembly.Location));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static FilePath GetPathFromCallingAssembly()
    {
        return FilePath.FromCallingAssembly();
    }

    private static void DeleteFileIfExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        File.SetAttributes(filePath, FileAttributes.Normal);
        File.Delete(filePath);
    }
}
