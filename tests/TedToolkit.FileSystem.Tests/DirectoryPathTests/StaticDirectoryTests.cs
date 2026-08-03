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
            if (Directory.Exists(temporaryDirectory.FullName))
            {
                File.SetAttributes(temporaryDirectory.FullName, FileAttributes.Normal);
                Directory.Delete(temporaryDirectory.FullName, true);
            }
        }
    }

    [Test]
    public async Task Should_expose_logical_drives_through_static_property()
    {
        var currentDrive = new DirectoryPath(Path.GetPathRoot(Environment.CurrentDirectory)!);

        var logicalDrives = DirectoryPath.GetLogicalDrives();

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
            if (Directory.Exists(temporaryDirectory.FullName))
            {
                File.SetAttributes(temporaryDirectory.FullName, FileAttributes.Normal);
                Directory.Delete(temporaryDirectory.FullName, true);
            }
        }
    }

    [Test]
    [Arguments(Environment.SpecialFolder.DesktopDirectory)]
    [Arguments(Environment.SpecialFolder.MyDocuments)]
    [Arguments(Environment.SpecialFolder.UserProfile)]
    [Arguments(Environment.SpecialFolder.ApplicationData)]
    [Arguments(Environment.SpecialFolder.LocalApplicationData)]
    [Arguments(Environment.SpecialFolder.CommonApplicationData)]
    public async Task Should_return_expected_special_folder_path_through_static_method(Environment.SpecialFolder specialFolder)
    {
        var expectedPath = Environment.GetFolderPath(specialFolder);

        var path = DirectoryPath.GetFolderPath(specialFolder);

        await Assert.That(path).IsEqualTo(new DirectoryPath(expectedPath));
    }

    [Test]
    public async Task Should_return_expected_special_folder_path_through_static_method_with_option()
    {
        var expectedPath = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData,
            Environment.SpecialFolderOption.DoNotVerify);

        var path = DirectoryPath.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.DoNotVerify);

        await Assert.That(path).IsEqualTo(new DirectoryPath(expectedPath));
    }

    [Test]
    public async Task Should_expose_desktop_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.Desktop)
            .IsEqualTo(new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)));
    }

    [Test]
    public async Task Should_expose_documents_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.Documents)
            .IsEqualTo(new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
    }

    [Test]
    public async Task Should_expose_user_profile_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.UserProfile)
            .IsEqualTo(new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)));
    }

    [Test]
    public async Task Should_expose_application_data_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.ApplicationData)
            .IsEqualTo(new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
    }

    [Test]
    public async Task Should_expose_local_application_data_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.LocalApplicationData)
            .IsEqualTo(new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
    }

    [Test]
    public async Task Should_expose_common_application_data_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.CommonApplicationData)
            .IsEqualTo(new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)));
    }

    [Test]
    public async Task Should_expose_temporary_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.Temp).IsEqualTo(new DirectoryPath(Path.GetTempPath()));
    }

    [Test]
    public async Task Should_expose_system_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.System).IsEqualTo(new DirectoryPath(Environment.SystemDirectory));
    }

    [Test]
    public async Task Should_expose_base_directory_through_static_property()
    {
        await Assert.That(DirectoryPath.BaseDirectory).IsEqualTo(new DirectoryPath(AppContext.BaseDirectory));
    }

    /// <summary>
    /// Verifies that a non-empty process environment variable is returned as a directory path.
    /// </summary>
    [Test]
    public async Task Should_return_directory_path_when_environment_variable_contains_value()
    {
        var variable = $"TEDTOOLKIT_FILESYSTEM_{Guid.NewGuid():N}";
        var expectedPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

        try
        {
            Environment.SetEnvironmentVariable(variable, expectedPath);

            var path = DirectoryPath.GetEnvironmentVariable(variable);

            await Assert.That(path).IsEqualTo(new DirectoryPath(expectedPath));
        }
        finally
        {
            Environment.SetEnvironmentVariable(variable, null);
        }
    }

    /// <summary>
    /// Verifies that a missing process environment variable does not produce a directory path.
    /// </summary>
    [Test]
    public async Task Should_return_null_when_environment_variable_is_missing()
    {
        var variable = $"TEDTOOLKIT_FILESYSTEM_{Guid.NewGuid():N}";

        var path = DirectoryPath.GetEnvironmentVariable(variable);

        await Assert.That(path).IsNull();
    }

    /// <summary>
    /// Verifies that an empty process environment variable does not produce a directory path.
    /// </summary>
    [Test]
    public async Task Should_return_null_when_environment_variable_is_empty()
    {
        var variable = $"TEDTOOLKIT_FILESYSTEM_{Guid.NewGuid():N}";

        try
        {
            Environment.SetEnvironmentVariable(variable, "");

            var path = DirectoryPath.GetEnvironmentVariable(variable);

            await Assert.That(path).IsNull();
        }
        finally
        {
            Environment.SetEnvironmentVariable(variable, null);
        }
    }

    /// <summary>
    /// Verifies that variable-name validation remains delegated to the environment API.
    /// </summary>
    [Test]
    public async Task Should_throw_argument_null_exception_when_environment_variable_name_is_null()
    {
        await Assert.That(() => DirectoryPath.GetEnvironmentVariable(null!))
            .Throws<ArgumentNullException>();
    }
}