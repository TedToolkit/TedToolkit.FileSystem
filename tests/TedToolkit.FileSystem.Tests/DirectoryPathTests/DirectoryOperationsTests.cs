// -----------------------------------------------------------------------
// <copyright file="DirectoryOperationsTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.DirectoryPathTests;

internal sealed class DirectoryOperationsTests
{
    [Test]
    public async Task Should_report_whether_directory_exists()
    {
        var existingPath = new DirectoryPath(TestAssets.RootDirectory.FullName);
        var missingPath = new DirectoryPath(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));

        await Assert.That(existingPath.Exists).IsTrue();
        await Assert.That(missingPath.Exists).IsFalse();
    }

    [Test]
    public async Task Should_create_directory()
    {
        var path = CreateTemporaryDirectoryPath();

        try
        {
            var createdDirectory = path.Create();

            await Assert.That(path.Exists).IsTrue().ConfigureAwait(false);
            await Assert.That(createdDirectory.FullName).IsEqualTo(path.FullName).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_delete_directory()
    {
        var path = CreateCreatedTemporaryDirectoryPath();

        path.Delete();

        await Assert.That(path.Exists).IsFalse().ConfigureAwait(false);
    }

    [Test]
    public async Task Should_delete_directory_recursively_when_requested()
    {
        var path = CreateCreatedTemporaryDirectoryPath();
        Directory.CreateDirectory(Path.Combine(path.FullName, "child"));

        path.Delete(recursive: true);

        await Assert.That(path.Exists).IsFalse().ConfigureAwait(false);
    }

    [Test]
    public async Task Should_move_directory_to_destination()
    {
        var source = CreateCreatedTemporaryDirectoryPath();
        var destination = CreateTemporaryDirectoryPath();

        try
        {
            source.MoveTo(destination);

            await Assert.That(source.Exists).IsFalse().ConfigureAwait(false);
            await Assert.That(destination.Exists).IsTrue().ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(source.FullName);
            DeleteDirectoryIfExists(destination.FullName);
        }
    }

    [Test]
    public async Task Should_get_and_set_creation_time_through_property()
    {
        var path = CreateCreatedTemporaryDirectoryPath();
        var expectedTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Local);

        try
        {
            path.CreationTime = expectedTime;

            await Assert.That(path.CreationTime).IsEqualTo(Directory.GetCreationTime(path.FullName)).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_get_and_set_last_write_time_through_property()
    {
        var path = CreateCreatedTemporaryDirectoryPath();
        var expectedTime = new DateTime(2024, 2, 3, 4, 5, 6, DateTimeKind.Local);

        try
        {
            path.LastWriteTime = expectedTime;

            await Assert.That(path.LastWriteTime).IsEqualTo(Directory.GetLastWriteTime(path.FullName)).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_get_and_set_last_access_time_through_property()
    {
        var path = CreateCreatedTemporaryDirectoryPath();
        var expectedTime = new DateTime(2024, 3, 4, 5, 6, 7, DateTimeKind.Local);

        try
        {
            path.LastAccessTime = expectedTime;

            await Assert.That(path.LastAccessTime).IsEqualTo(Directory.GetLastAccessTime(path.FullName)).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_get_and_set_creation_time_utc_through_property()
    {
        var path = CreateCreatedTemporaryDirectoryPath();
        var expectedTime = new DateTime(2024, 4, 5, 6, 7, 8, DateTimeKind.Utc);

        try
        {
            path.CreationTimeUtc = expectedTime;

            await Assert.That(path.CreationTimeUtc).IsEqualTo(Directory.GetCreationTimeUtc(path.FullName)).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_get_and_set_last_write_time_utc_through_property()
    {
        var path = CreateCreatedTemporaryDirectoryPath();
        var expectedTime = new DateTime(2024, 5, 6, 7, 8, 9, DateTimeKind.Utc);

        try
        {
            path.LastWriteTimeUtc = expectedTime;

            await Assert.That(path.LastWriteTimeUtc).IsEqualTo(Directory.GetLastWriteTimeUtc(path.FullName)).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_get_and_set_last_access_time_utc_through_property()
    {
        var path = CreateCreatedTemporaryDirectoryPath();
        var expectedTime = new DateTime(2024, 6, 7, 8, 9, 10, DateTimeKind.Utc);

        try
        {
            path.LastAccessTimeUtc = expectedTime;

            await Assert.That(path.LastAccessTimeUtc).IsEqualTo(Directory.GetLastAccessTimeUtc(path.FullName)).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_get_and_set_attributes_through_property()
    {
        var path = CreateCreatedTemporaryDirectoryPath();

        try
        {
            path.Attributes = FileAttributes.ReadOnly;

            await Assert.That(path.Attributes).IsEqualTo(File.GetAttributes(path.FullName)).ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_create_directory_with_unix_file_mode_when_supported()
    {
        var path = CreateTemporaryDirectoryPath();

        try
        {
            if (OperatingSystem.IsWindows())
            {
                await Assert.That(() => CreateDirectoryViaReflection(path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute))
                    .Throws<PlatformNotSupportedException>()
                    .ConfigureAwait(false);
                return;
            }

            var createdDirectory = CreateDirectory(path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);

            await Assert.That(createdDirectory).IsEqualTo(path).ConfigureAwait(false);
            await Assert.That(path.Exists).IsTrue().ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(path.FullName);
        }
    }

    [Test]
    public async Task Should_create_symbolic_link_and_resolve_target_when_supported()
    {
        var workspace = CreateCreatedTemporaryDirectoryPath();
        var target = workspace / "target";
        var link = workspace / "link";
        target.Create();

        try
        {
            var linkWasCreated = false;

            try
            {
                var createdLink = link.CreateSymbolicLink(target);
                linkWasCreated = true;

                await Assert.That(createdLink).IsEqualTo(link).ConfigureAwait(false);
                await Assert.That(link.ResolveLinkTarget(returnFinalTarget: false)).IsEqualTo(target).ConfigureAwait(false);
                await Assert.That(link.ResolveLinkTarget(returnFinalTarget: true)).IsEqualTo(target).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
                await Assert.That(OperatingSystem.IsWindows()).IsTrue().ConfigureAwait(false);
            }
            catch (IOException)
            {
                await Assert.That(OperatingSystem.IsWindows()).IsTrue().ConfigureAwait(false);
            }

            await Assert.That(linkWasCreated || OperatingSystem.IsWindows()).IsTrue().ConfigureAwait(false);
        }
        finally
        {
            DeleteDirectoryIfExists(workspace.FullName);
        }
    }

    [Test]
    public async Task Should_set_current_directory()
    {
        var originalCurrentDirectory = Environment.CurrentDirectory;
        var path = new DirectoryPath(TestAssets.RootDirectory.FullName);

        try
        {
            path.SetCurrentDirectory();

            await Assert.That(Environment.CurrentDirectory).IsEqualTo(path.FullName).ConfigureAwait(false);
        }
        finally
        {
            Environment.CurrentDirectory = originalCurrentDirectory;
        }
    }

    [Test]
    public async Task Should_return_same_text_from_to_string()
    {
        var path = new DirectoryPath(TestAssets.NestedDirectory.FullName);

        await Assert.That(path.ToString()).IsEqualTo(path.FullName).ConfigureAwait(false);
    }

    [Test]
    public async Task Should_throw_argument_null_exception_when_creating_from_null_directory_info()
    {
        DirectoryInfo directoryInfo = null!;

        await Assert.That(() => DirectoryPath.FromDirectoryInfo(directoryInfo))
            .Throws<ArgumentNullException>()
            .ConfigureAwait(false);
    }

    [Test]
    public async Task Should_throw_argument_null_exception_when_implicitly_converting_null_directory_info()
    {
        DirectoryInfo directoryInfo = null!;

        await Assert.That(() => (DirectoryPath)directoryInfo)
            .Throws<ArgumentNullException>()
            .ConfigureAwait(false);
    }

    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macos")]
    private static DirectoryPath CreateDirectory(DirectoryPath path, UnixFileMode unixFileMode)
    {
        return path.Create(unixFileMode);
    }

    private static void CreateDirectoryViaReflection(DirectoryPath path, UnixFileMode unixFileMode)
    {
        try
        {
            typeof(DirectoryPath).GetMethod(nameof(DirectoryPath.Create), [typeof(UnixFileMode)])!.Invoke(path, [unixFileMode]);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
        }
    }

    private static DirectoryPath CreateTemporaryDirectoryPath()
    {
        return new(Path.Combine(Path.GetTempPath(), "TedToolkit.FileSystem.Tests", Guid.NewGuid().ToString("N")));
    }

    private static DirectoryPath CreateCreatedTemporaryDirectoryPath()
    {
        var path = CreateTemporaryDirectoryPath();
        Directory.CreateDirectory(path.FullName);
        return path;
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
