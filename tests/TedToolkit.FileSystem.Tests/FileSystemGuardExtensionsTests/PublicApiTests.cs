// -----------------------------------------------------------------------
// <copyright file="PublicApiTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;

using TedToolkit.FileSystem;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

internal sealed class PublicApiTests
{
    /// <summary>
    /// Verifies that extensions cannot apply to unrelated objects, strings, collections, or lazy sequences.
    /// </summary>
    [Test]
    public async Task Should_limit_extension_receivers_to_filesystem_types()
    {
        Type[] allowedReceivers = [typeof(FilePath), typeof(FilePath?), typeof(DirectoryPath), typeof(DirectoryPath?),
            typeof(IReadOnlyCollection<FilePath>), typeof(IReadOnlyCollection<DirectoryPath>),];
        var methods = typeof(FileSystemGuardExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);
        await Assert.That(methods.Length).IsEqualTo(24);
        await Assert.That(methods.All(method => !method.IsGenericMethod
            && method.ReturnType == typeof(void)
            && method.IsDefined(typeof(ExtensionAttribute))
            && allowedReceivers.Contains(method.GetParameters()[0].ParameterType))).IsTrue();
    }

    /// <summary>
    /// Verifies that translation generation replaces the independent resource implementation.
    /// </summary>
    [Test]
    public async Task Should_remove_independent_resource_helpers()
    {
        var helper = typeof(FileSystemGuardExtensions).Assembly.GetType("TedToolkit.FileSystem.FileSystemGuardMessages");
        await Assert.That(helper).IsNull();
        await Assert.That(typeof(FileSystemGuardExtensions).Assembly.GetManifestResourceNames()).IsEmpty();
    }
}