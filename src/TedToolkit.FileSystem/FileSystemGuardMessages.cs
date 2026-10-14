// -----------------------------------------------------------------------
// <copyright file="FileSystemGuardMessages.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Resources;

using TedToolkit.Localizations;

namespace TedToolkit.FileSystem;

/// <summary>
/// Provides internal localized messages for filesystem guard failures.
/// </summary>
internal static class FileSystemGuardMessages
{
    private static readonly ResourceManager Messages = new(
        typeof(FileSystemGuardMessages).FullName!, typeof(FileSystemGuardMessages).Assembly);

    /// <summary>
    /// Formats a failure message using the selected TedToolkit language.
    /// </summary>
    /// <param name="key">The internal resource key.</param>
    /// <param name="arguments">The caller expression, path, and other diagnostic values.</param>
    /// <returns>The formatted localized message.</returns>
    internal static string Format(string key, params string?[] arguments)
    {
        var template = Messages.GetString(key, GetCulture()) ?? key;
        return string.Format(CultureInfo.InvariantCulture, template, arguments);
    }

    private static CultureInfo GetCulture()
    {
        try
        {
            return CultureInfo.GetCultureInfo(LocalizationSettings.Culture ?? "");
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.InvariantCulture;
        }
    }
}