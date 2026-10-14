// -----------------------------------------------------------------------
// <copyright file="LocalizedMessagesTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

using TedToolkit.FileSystem;

using TedToolkit.Localizations;

namespace TedToolkit.FileSystem.Tests.FileSystemGuardExtensionsTests;

[NotInParallel("FileSystemGuardCulture")]
internal sealed class LocalizedMessagesTests
{
    /// <summary>
    /// Verifies that guard messages follow TedToolkit's culture rather than the ambient UI culture.
    /// </summary>
    [Test]
    [Arguments("en-US", "File parameter")]
    [Arguments("zh-CN", "文件参数")]
    [Arguments("zh-SG", "文件参数")]
    [Arguments("fr-FR", "File parameter")]
    [Arguments("not_a_culture", "File parameter")]
    public async Task Should_use_selected_language_and_fallback_without_losing_failure_details(string culture, string text)
    {
        using var fixture = new FileSystemGuardFixture();
        var originalCulture = LocalizationSettings.Culture;
        var originalUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            LocalizationSettings.Culture = culture;
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
            var inputFile = fixture.MissingFile;
            var exception = await Assert.That(() => inputFile.ThrowIfNotExists()).Throws<FileNotFoundException>();
            await Assert.That(exception!.Message).Contains(text);
            await Assert.That(exception.Message).Contains("inputFile");
            await Assert.That(exception.Message).Contains(inputFile.FullName);
            await Assert.That(exception.FileName).IsEqualTo(inputFile.FullName);
        }
        finally
        {
            LocalizationSettings.Culture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    /// <summary>
    /// Verifies that Chinese resources cover every filesystem guard failure and preserve exception types.
    /// </summary>
    [Test]
    public async Task Should_localize_all_filesystem_failures_when_chinese_is_selected()
    {
        using var fixture = new FileSystemGuardFixture();
        var originalCulture = LocalizationSettings.Culture;
        try
        {
            LocalizationSettings.Culture = "zh-CN";
            FilePath? inputFile = null;
            DirectoryPath? nullFolder = null;
            FilePath[] files = [];
            DirectoryPath[] folders = [];
            var inputFolder = fixture.DirectoryPath;
            var nullException = await Assert.That(() => inputFile.ThrowIfNull()).Throws<ArgumentNullException>();
            var nullFolderException = await Assert.That(() => nullFolder.ThrowIfNull()).Throws<ArgumentNullException>();
            var missingException = await Assert.That(() => fixture.MissingFolder.ThrowIfNotExists())
                .Throws<DirectoryNotFoundException>();
            var existsException = await Assert.That(() => inputFolder.ThrowIfExists()).Throws<IOException>();
            var filesException = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles())
                .Throws<InvalidOperationException>();
            var patternException = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles("*.json"))
                .Throws<InvalidOperationException>();
            var foldersException = await Assert.That(() => inputFolder.ThrowIfContainsNoFolders())
                .Throws<InvalidOperationException>();
            var entriesException = await Assert.That(() => inputFolder.ThrowIfContainsNoEntries())
                .Throws<InvalidOperationException>();
            var itemsException = await Assert.That(() => files.ThrowIfNullOrHasNoItems()).Throws<ArgumentException>();
            var folderItemsException = await Assert.That(() => folders.ThrowIfNullOrHasNoItems()).Throws<ArgumentException>();
            var blankException = await Assert.That(() => inputFolder.ThrowIfContainsNoFiles(" ")).Throws<ArgumentException>();
            await Assert.That(nullException!.Message).Contains("不能为 null");
            await Assert.That(nullFolderException!.Message).Contains("不能为 null");
            await Assert.That(missingException!.Message).Contains("目录不存在");
            await Assert.That(existsException!.Message).Contains("路径已被占用");
            await Assert.That(filesException!.Message).Contains("未找到匹配");
            await Assert.That(patternException!.Message).Contains("未找到匹配");
            await Assert.That(foldersException!.Message).Contains("未找到匹配");
            await Assert.That(entriesException!.Message).Contains("既没有文件");
            await Assert.That(itemsException!.Message).Contains("不包含任何元素");
            await Assert.That(folderItemsException!.Message).Contains("不包含任何元素");
            await Assert.That(blankException!.Message).Contains("空字符串或全为空白");
        }
        finally
        {
            LocalizationSettings.Culture = originalCulture;
        }
    }
}