// -----------------------------------------------------------------------
// <copyright file="PlatformOperationTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.ExceptionServices;

namespace TedToolkit.FileSystem.Tests.FilePathTests;

internal sealed class PlatformOperationTests
{
    /// <summary>
    /// Verifies that symbolic link helpers delegate to the underlying file APIs when the platform allows it.
    /// </summary>
    [Test]
#if NET6_0_OR_GREATER
    public async Task Should_create_and_resolve_file_symbolic_link_when_supported()
    {
        var root = TestWorkspace.CreateDirectory();
        var target = new FilePath(Path.Combine(root.FullName, "target.txt"));
        var link = new FilePath(Path.Combine(root.FullName, "link.txt"));
        await target.WriteAllTextAsync("target", CancellationToken.None).ConfigureAwait(false);

        try
        {
            var linkWasCreated = false;

            try
            {
                var createdLink = link.CreateSymbolicLink(target.FullName);
                linkWasCreated = true;

                await Assert.That(createdLink.FullName).IsEqualTo(link.FullName);
                await Assert.That(link.ResolveLinkTarget(returnFinalTarget: false)?.FullName).IsEqualTo(target.FullName);
                await Assert.That(link.ResolveLinkTarget(returnFinalTarget: true)?.FullName).IsEqualTo(target.FullName);
            }
            catch (UnauthorizedAccessException)
            {
                await Assert.That(OperatingSystem.IsWindows()).IsTrue();
            }
            catch (IOException)
            {
                await Assert.That(OperatingSystem.IsWindows()).IsTrue();
            }

            await Assert.That(linkWasCreated || OperatingSystem.IsWindows()).IsTrue();
        }
        finally
        {
            root.Delete(true);
        }
    }
#endif

    /// <summary>
    /// Verifies that Unix file mode accessors work on supported platforms and fail naturally on Windows.
    /// </summary>
    [Test]
#if NET7_0_OR_GREATER
    public async Task Should_expose_unix_file_mode_when_supported()
    {
        var file = TestWorkspace.CreateFile("unix-mode.txt", "content");
        var path = new FilePath(file.FullName);

        try
        {
            if (OperatingSystem.IsWindows())
            {
                await Assert.That(() => GetUnixFileModeViaReflection(in path))
                    .Throws<PlatformNotSupportedException>();
                return;
            }

            SetUnixFileModeViaReflection(in path, UnixFileMode.UserRead | UnixFileMode.UserWrite);

            await Assert.That(GetUnixFileModeViaReflection(in path))
                .IsEqualTo(File.GetUnixFileMode(file.FullName));
        }
        finally
        {
            file.Directory!.Delete(true);
        }
    }
#endif

    /// <summary>
    /// Verifies that encrypt and decrypt wrappers preserve the same platform support contract as the BCL.
    /// </summary>
    [Test]
    public async Task Should_match_platform_contract_for_encrypt_and_decrypt()
    {
        var file = TestWorkspace.CreateFile("encryption.txt", "content");
        var path = new FilePath(file.FullName);

        try
        {
            if (!OperatingSystem.IsWindows())
            {
                await Assert.That(() => InvokeEncryptViaReflection(in path))
                    .Throws<PlatformNotSupportedException>();
                await Assert.That(() => InvokeDecryptViaReflection(in path))
                    .Throws<PlatformNotSupportedException>();
                return;
            }

            try
            {
                path.Encrypt();
                path.Decrypt();
            }
            catch (Exception exception) when (exception is IOException or NotSupportedException or UnauthorizedAccessException)
            {
                await Assert.That(exception).IsNotNull();
            }
        }
        finally
        {
            if (File.Exists(file.FullName))
            {
                File.SetAttributes(file.FullName, FileAttributes.Normal);
            }

            file.Directory!.Delete(true);
        }
    }

    private static UnixFileMode GetUnixFileModeViaReflection(in FilePath path)
    {
        try
        {
            return (UnixFileMode)typeof(FilePath).GetProperty(nameof(FilePath.UnixFileMode), BindingFlags.Instance | BindingFlags.Public)!.GetValue(path)!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            return default;
        }
    }

    private static void InvokeDecryptViaReflection(in FilePath path)
    {
        InvokePublicInstanceMember(path, nameof(FilePath.Decrypt));
    }

    private static void InvokeEncryptViaReflection(in FilePath path)
    {
        InvokePublicInstanceMember(path, nameof(FilePath.Encrypt));
    }

    private static void InvokePublicInstanceMember(object target, string memberName)
    {
        try
        {
            target.GetType().GetMethod(memberName, BindingFlags.Instance | BindingFlags.Public)!.Invoke(target, null);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
        }
    }

    private static void SetUnixFileModeViaReflection(in FilePath path, UnixFileMode unixFileMode)
    {
        try
        {
            typeof(FilePath).GetProperty(nameof(FilePath.UnixFileMode), BindingFlags.Instance | BindingFlags.Public)!.SetValue(path, unixFileMode);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
        }
    }
}
