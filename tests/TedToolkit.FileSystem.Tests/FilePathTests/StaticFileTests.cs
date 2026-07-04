// -----------------------------------------------------------------------
// <copyright file="StaticFileTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
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
        var path = ReadExecutingAssemblyPath();

        await Assert.That(path).IsEqualTo(new FilePath(typeof(FilePath).Assembly.Location));
    }

    [Test]
    public async Task Should_expose_assembly_factories_as_static_properties()
    {
        await AssertAssemblyPropertyContract("EntryAssembly", typeof(FilePath?)).ConfigureAwait(false);
        await AssertAssemblyPropertyContract("ExecutingAssembly", typeof(FilePath)).ConfigureAwait(false);
        await AssertAssemblyPropertyContract("CallingAssembly", typeof(FilePath)).ConfigureAwait(false);
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
        var expectedAssembly = Assembly.GetEntryAssembly();
        var path = ReadEntryAssemblyPath();

        if (expectedAssembly is null)
        {
            await Assert.That(path).IsEqualTo(default(FilePath?));
            return;
        }

        await Assert.That(path).IsEqualTo(new FilePath(expectedAssembly.Location));
    }

    [Test]
    public async Task Should_throw_argument_null_exception_when_creating_file_path_from_null_assembly()
    {
        Assembly assembly = null!;

        await Assert.That(() => FilePath.FromAssembly(assembly))
            .Throws<ArgumentNullException>();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static FilePath GetPathFromCallingAssembly()
    {
        return ReadCallingAssemblyPath();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static FilePath ReadExecutingAssemblyPath()
    {
        return FilePath.ExecutingAssembly;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static FilePath? ReadEntryAssemblyPath()
    {
        return FilePath.EntryAssembly;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static FilePath ReadCallingAssemblyPath()
    {
        return FilePath.CallingAssembly;
    }

    private static async Task AssertAssemblyPropertyContract(string propertyName, Type propertyType)
    {
        var property = typeof(FilePath).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);
        var method = typeof(FilePath).GetMethod($"From{propertyName}", BindingFlags.Public | BindingFlags.Static);

        await Assert.That(property).IsNotNull();
        await Assert.That(property!.PropertyType).IsEqualTo(propertyType);
        await Assert.That(method).IsNull();
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
