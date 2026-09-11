using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

using TedToolkit.FileSystem;
using TedToolkit.FileSystem.ProjectPaths;

namespace TedToolkit.FileSystem.ProjectPaths.Tests;

internal sealed class ProjectPathsGeneratorTests
{
    /// <summary>
    /// Verifies that explicitly selected files generate only their required access chain and safe member name.
    /// </summary>
    [Test]
    public async Task Should_generate_safe_hierarchical_member_when_selected_file_contains_spaces_and_dots()
    {
        var gitDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var projectFile = Path.Combine(gitDirectory, "src", "App", "App.csproj");
        var selectedFile = Path.Combine(gitDirectory, "src", "App", "my  file.name.json");
        Directory.CreateDirectory(Path.GetDirectoryName(projectFile)!);
        await File.WriteAllTextAsync(projectFile, "<Project />");
        await File.WriteAllTextAsync(selectedFile, "{}");

        try
        {
            var result = Generate(gitDirectory, projectFile, "src/App/my  file.name.json~File~");
            var source = result.GeneratedSources.Single();

            await Assert.That(source.Contains("public static class ProjectPaths")).IsTrue();
            await Assert.That(source.Contains("DirectoryPath Git")).IsTrue();
            await Assert.That(source.Contains("public static class src")).IsTrue();
            await Assert.That(source.Contains("public static class App")).IsTrue();
            await Assert.That(source.Contains("my_file_name_json")).IsTrue();
            await Assert.That(source.Contains("my__file")).IsFalse();
            await Assert.That(source.Contains("Full path:")).IsTrue();
            await Assert.That(result.CompilationDiagnostics.Any(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)).IsFalse();
        }
        finally
        {
            Directory.Delete(gitDirectory, recursive: true);
        }
    }

    /// <summary>
    /// Verifies that concrete selected files generate members at every represented descendant level.
    /// </summary>
    [Test]
    public async Task Should_generate_concrete_files_in_descendant_directories()
    {
        var gitDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var projectFile = Path.Combine(gitDirectory, "src", "App", "App.csproj");
        var rootJson = Path.Combine(gitDirectory, "src", "App", "appsettings.json");
        var nestedJson = Path.Combine(gitDirectory, "src", "App", "Settings", "feature.json");
        var ignoredFile = Path.Combine(gitDirectory, "src", "App", "Settings", "feature.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(nestedJson)!);
        await File.WriteAllTextAsync(projectFile, "<Project />");
        await File.WriteAllTextAsync(rootJson, "{}");
        await File.WriteAllTextAsync(nestedJson, "{}");
        await File.WriteAllTextAsync(ignoredFile, string.Empty);

        try
        {
            var result = Generate(gitDirectory, projectFile, "src/App/appsettings.json~File~|src/App/Settings/feature.json~File~");
            var source = result.GeneratedSources.Single();

            await Assert.That(source.Contains("appsettings_json")).IsTrue();
            await Assert.That(source.Contains("public static class Settings")).IsTrue();
            await Assert.That(source.Contains("feature_json")).IsTrue();
            await Assert.That(source.Contains("feature_txt")).IsFalse();
            await Assert.That(result.CompilationDiagnostics.Any(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)).IsFalse();
        }
        finally
        {
            Directory.Delete(gitDirectory, recursive: true);
        }
    }

    /// <summary>
    /// Verifies that unselected files in child directories are not generated.
    /// </summary>
    [Test]
    public async Task Should_exclude_unselected_files_in_descendant_directories()
    {
        var gitDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var projectFile = Path.Combine(gitDirectory, "src", "App", "App.csproj");
        var rootJson = Path.Combine(gitDirectory, "src", "App", "appsettings.json");
        var nestedJson = Path.Combine(gitDirectory, "src", "App", "Settings", "feature.json");
        Directory.CreateDirectory(Path.GetDirectoryName(nestedJson)!);
        await File.WriteAllTextAsync(projectFile, "<Project />");
        await File.WriteAllTextAsync(rootJson, "{}");
        await File.WriteAllTextAsync(nestedJson, "{}");

        try
        {
            var result = Generate(gitDirectory, projectFile, "src/App/appsettings.json~File~");
            var source = result.GeneratedSources.Single();

            await Assert.That(source.Contains("appsettings_json")).IsTrue();
            await Assert.That(source.Contains("feature_json")).IsFalse();
            await Assert.That(source.Contains("public static class Settings")).IsFalse();
        }
        finally
        {
            Directory.Delete(gitDirectory, recursive: true);
        }
    }

    /// <summary>
    /// Verifies that selecting a directory does not implicitly generate files below it.
    /// </summary>
    [Test]
    public async Task Should_generate_only_directory_member_when_directory_is_selected()
    {
        var gitDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var projectFile = Path.Combine(gitDirectory, "src", "App", "App.csproj");
        var assetFile = Path.Combine(gitDirectory, "src", "App", "Assets", "logo.svg");
        Directory.CreateDirectory(Path.GetDirectoryName(assetFile)!);
        await File.WriteAllTextAsync(projectFile, "<Project />");
        await File.WriteAllTextAsync(assetFile, "<svg />");

        try
        {
            var result = Generate(gitDirectory, projectFile, "src/App/Assets~Directory~");
            var source = result.GeneratedSources.Single();

            await Assert.That(source.Contains("public static class Assets")).IsTrue();
            await Assert.That(source.Contains("DirectoryPath Directory")).IsTrue();
            await Assert.That(source.Contains("logo_svg")).IsFalse();
        }
        finally
        {
            Directory.Delete(gitDirectory, recursive: true);
        }
    }

    /// <summary>
    /// Verifies that colliding generated file names report the documented diagnostic.
    /// </summary>
    [Test]
    public async Task Should_report_name_collision_when_selected_files_generate_the_same_member_name()
    {
        var gitDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var projectFile = Path.Combine(gitDirectory, "src", "App", "App.csproj");
        Directory.CreateDirectory(Path.GetDirectoryName(projectFile)!);
        await File.WriteAllTextAsync(projectFile, "<Project />");
        await File.WriteAllTextAsync(Path.Combine(gitDirectory, "src", "App", "one.file.json"), "{}");
        await File.WriteAllTextAsync(Path.Combine(gitDirectory, "src", "App", "one-file.json"), "{}");

        try
        {
            var result = Generate(gitDirectory, projectFile, "src/App/one.file.json~File~|src/App/one-file.json~File~");

            await Assert.That(result.GeneratorDiagnostics.Any(static diagnostic => diagnostic.Id == "TTFS002")).IsTrue();
        }
        finally
        {
            Directory.Delete(gitDirectory, recursive: true);
        }
    }

    /// <summary>
    /// Verifies that a missing Git work tree reports the documented diagnostic without generating source.
    /// </summary>
    [Test]
    public async Task Should_report_missing_git_root_when_git_directory_does_not_exist()
    {
        var gitDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var projectFile = Path.Combine(gitDirectory, "App.csproj");

        var result = Generate(gitDirectory, projectFile, "src/App/*.json~File~");

        await Assert.That(result.GeneratorDiagnostics.Any(static diagnostic => diagnostic.Id == "TTFS001")).IsTrue();
        await Assert.That(result.GeneratedSources).IsEmpty();
    }

    private static GeneratorResult Generate(string gitDirectory, string projectFile, string paths)
    {
        var options = new TestAnalyzerConfigOptionsProvider(new Dictionary<string, string>
        {
            ["build_property.RootNamespace"] = "Generated",
            ["build_property.TedToolkitFileSystemGitDirectory"] = gitDirectory,
            ["build_property.TedToolkitFileSystemProjectFile"] = projectFile,
            ["build_property.TedToolkitFileSystemPaths"] = paths,
        });
        var compilation = CSharpCompilation.Create(
            "GeneratedPaths",
            references: GetFrameworkReferences(),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: [new ProjectPathsGenerator().AsSourceGenerator()],
            optionsProvider: options);

        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _);
        var runResult = driver.GetRunResult();
        return new GeneratorResult(
            runResult.GeneratedTrees.Select(static tree => tree.GetText().ToString()).ToImmutableArray(),
            runResult.Diagnostics,
            outputCompilation.GetDiagnostics());
    }

    private static IEnumerable<MetadataReference> GetFrameworkReferences()
    {
        var trustedPlatformAssemblies = (string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!;
        return trustedPlatformAssemblies
            .Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
    }

    private sealed record GeneratorResult(
        ImmutableArray<string> GeneratedSources,
        ImmutableArray<Diagnostic> GeneratorDiagnostics,
        ImmutableArray<Diagnostic> CompilationDiagnostics);

    private sealed class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
    {
        public TestAnalyzerConfigOptionsProvider(IReadOnlyDictionary<string, string> values)
        {
            GlobalOptions = new TestAnalyzerConfigOptions(values);
        }

        public override AnalyzerConfigOptions GlobalOptions { get; }

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => EmptyAnalyzerConfigOptions.Instance;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => EmptyAnalyzerConfigOptions.Instance;
    }

    private sealed class TestAnalyzerConfigOptions(IReadOnlyDictionary<string, string> values) : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, out string value) => values.TryGetValue(key, out value!);
    }

    private sealed class EmptyAnalyzerConfigOptions : AnalyzerConfigOptions
    {
        public static EmptyAnalyzerConfigOptions Instance { get; } = new();

        public override bool TryGetValue(string key, out string value)
        {
            value = string.Empty;
            return false;
        }
    }
}
