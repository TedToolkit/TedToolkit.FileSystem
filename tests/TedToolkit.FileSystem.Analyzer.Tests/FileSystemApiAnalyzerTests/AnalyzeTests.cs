using System.Collections.Immutable;
using System.Globalization;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace TedToolkit.FileSystem.Analyzer.Tests.FileSystemApiAnalyzerTests;

internal sealed class AnalyzeTests
{
    private static readonly ImmutableArray<MetadataReference> _references =
        ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
        .Select(static path => (MetadataReference)MetadataReference.CreateFromFile(path))
        .Append(MetadataReference.CreateFromFile(typeof(FilePath).Assembly.Location))
        .ToImmutableArray();

    /// <summary>
    /// Verifies that every blocked BCL type reports an error with its path replacement.
    /// </summary>
    [Test]
    [Arguments("System.IO.Directory.Exists(\"x\")", "Directory", "use TedToolkit.FileSystem.DirectoryPath")]
    [Arguments("System.IO.File.Exists(\"x\")", "File", "use TedToolkit.FileSystem.FilePath")]
    [Arguments("System.IO.Path.Combine(\"x\", \"y\")", "Path", "use TedToolkit.FileSystem.DirectoryPath for directories or TedToolkit.FileSystem.FilePath for files")]
    [Arguments("new System.IO.FileInfo(\"x\")", "FileInfo", "use TedToolkit.FileSystem.FilePath")]
    [Arguments("new System.IO.DirectoryInfo(\"x\")", "DirectoryInfo", "use TedToolkit.FileSystem.DirectoryPath")]
    [Arguments("System.IO.Path.DirectorySeparatorChar", "Path", "use TedToolkit.FileSystem.DirectoryPath for directories or TedToolkit.FileSystem.FilePath for files")]
    public async Task Should_report_error_and_replacement_when_blocked_type_is_used(string expression, string typeName, string replacement)
    {
        var diagnostics = await AnalyzeAsync("class Sample { object M() => " + expression + "; }");

        await Assert.That(diagnostics.Length).IsEqualTo(1);
        var diagnostic = diagnostics.Single();
        await Assert.That(diagnostic.Id).IsEqualTo(FileSystemApiAnalyzer.DiagnosticId);
        await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(diagnostic.GetMessage(CultureInfo.InvariantCulture)).IsEqualTo($"Do not use System.IO.{typeName} directly; {replacement}");
        await Assert.That(diagnostic.Location.SourceTree!.GetText().ToString(diagnostic.Location.SourceSpan)).IsEqualTo(typeName);
    }

    /// <summary>
    /// Verifies that aliases, declarations, method groups, and inferred member access cannot bypass the rule.
    /// </summary>
    [Test]
    [Arguments("using Storage = System.IO.File; class Sample { bool M() => Storage.Exists(\"x\"); }", 2)]
    [Arguments("using IO = System.IO; class Sample { bool M() => IO.File.Exists(\"x\"); }", 1)]
    [Arguments("using static System.IO.File; class Sample { bool M() => Exists(\"x\"); }", 2)]
    [Arguments("using static System.IO.Path; class Sample { char M() => DirectorySeparatorChar; }", 2)]
    [Arguments("class Sample { System.Func<string, bool> M() => System.IO.File.Exists; }", 1)]
    [Arguments("using static System.IO.File; class Sample { System.Func<string, bool> M() => Exists; }", 2)]
    [Arguments("class Sample { System.IO.FileInfo M() => new(\"x\"); }", 2)]
    [Arguments("class Sample { void M(System.IO.FileInfo file) { file.Delete(); } }", 2)]
    [Arguments("class Sample { bool M(System.IO.FileInfo file) => file.Exists; }", 2)]
    [Arguments("class Sample { string M(System.IO.DirectoryInfo directory) => directory.FullName; }", 2)]
    [Arguments("class Sample { bool? M(System.IO.FileInfo file) => file?.Exists; }", 2)]
    [Arguments("class Sample { System.IO.FileInfo File { get; set; } }", 1)]
    [Arguments("class Sample { System.Collections.Generic.List<System.IO.DirectoryInfo[]> Directories; }", 1)]
    [Arguments("class Sample { System.Type M() => typeof(global::System.IO.FileInfo); }", 1)]
    [Arguments("class Sample { string M() => nameof(System.IO.File); }", 1)]
    [Arguments("class Sample { bool M() => new System.IO.FileInfo(\"x\").Exists; }", 2)]
    [Arguments("class Sample { bool M(string path) => System.IO.File.Exists(System.IO.Path.GetFullPath(path)); }", 2)]
    public async Task Should_report_references_when_alternative_syntax_is_used(string source, int expectedCount)
    {
        var diagnostics = await AnalyzeAsync(source);

        await Assert.That(diagnostics.Length).IsEqualTo(expectedCount);
        await Assert.That(diagnostics.All(static diagnostic => diagnostic.Id == FileSystemApiAnalyzer.DiagnosticId && diagnostic.Severity == DiagnosticSeverity.Error)).IsTrue();
    }

    /// <summary>
    /// Verifies that inherited members are still blocked when an external API returns a file system info instance.
    /// </summary>
    [Test]
    public async Task Should_report_error_when_external_api_returns_blocked_instance()
    {
        var external = CreateCompilation("public static class External { public static System.IO.FileInfo GetFile() => new System.IO.FileInfo(\"x\"); }", "External");
        var diagnostics = await AnalyzeAsync("class Sample { bool M() => External.GetFile().Exists; }", external.ToMetadataReference());

        await Assert.That(diagnostics.Length).IsEqualTo(1);
        await Assert.That(diagnostics.Single().GetMessage(CultureInfo.InvariantCulture)).IsEqualTo("Do not use System.IO.FileInfo directly; use TedToolkit.FileSystem.FilePath");
    }

    /// <summary>
    /// Verifies that path wrappers, unrelated IO APIs, and custom types with matching names remain usable.
    /// </summary>
    [Test]
    [Arguments("using System.IO; class Sample { string M() => \"System.IO.File.Exists\"; }")]
    [Arguments("class Sample { object M() => new System.IO.MemoryStream(); }")]
    [Arguments("class Sample { bool M(TedToolkit.FileSystem.FilePath file) => file.Exists; }")]
    [Arguments("class Sample { bool M(TedToolkit.FileSystem.DirectoryPath directory) => directory.Exists; }")]
    [Arguments("class Sample { string M(TedToolkit.FileSystem.FileName name) => name.ToString(); }")]
    [Arguments("namespace Custom { class File {} class Directory {} class Path {} class FileInfo {} class DirectoryInfo {} class Sample { File File; Directory Directory; Path Path; FileInfo FileInfo; DirectoryInfo DirectoryInfo; } }")]
    [Arguments("class Sample { public bool File { get; set; } public bool M() => File; }")]
    [Arguments("/// <summary>Wraps <see cref=\"System.IO.File\"/>.</summary>\nclass Sample { }")]
    [Arguments("class Sample { bool M() => false; /* System.IO.File.Exists(\"x\") */ }")]
    public async Task Should_ignore_source_when_no_blocked_reference_is_used(string source)
    {
        var diagnostics = await AnalyzeAsync(source);

        await Assert.That(diagnostics).IsEmpty();
    }

    /// <summary>
    /// Verifies that generated source and explicit suppression do not produce errors.
    /// </summary>
    [Test]
    [Arguments("// <auto-generated/>\nclass Sample { bool M() => System.IO.File.Exists(\"x\"); }", "Sample.cs")]
    [Arguments("class Sample { bool M() => System.IO.File.Exists(\"x\"); }", "Sample.g.cs")]
    [Arguments("#pragma warning disable TTFS003\nclass Sample { bool M() => System.IO.File.Exists(\"x\"); }", "Sample.cs")]
    public async Task Should_ignore_reference_when_generated_or_suppressed(string source, string path)
    {
        var diagnostics = await AnalyzeCompilationAsync(CreateCompilation(source, path: path));

        await Assert.That(diagnostics).IsEmpty();
    }

    private static Task<ImmutableArray<Diagnostic>> AnalyzeAsync(string source, MetadataReference? additionalReference = null)
    {
        var compilation = CreateCompilation(source);
        if (additionalReference is not null)
            compilation = compilation.AddReferences(additionalReference);

        return AnalyzeCompilationAsync(compilation);
    }

    private static CSharpCompilation CreateCompilation(string source, string assemblyName = "AnalyzerTest", string path = "Sample.cs") =>
        CSharpCompilation.Create(
            assemblyName,
            new[] { CSharpSyntaxTree.ParseText(source, path: path) },
            _references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    private static Task<ImmutableArray<Diagnostic>> AnalyzeCompilationAsync(CSharpCompilation compilation)
    {
        var errors = compilation.GetDiagnostics().Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();
        if (errors.Length != 0)
            throw new InvalidOperationException(string.Join(Environment.NewLine, errors.Select(static diagnostic => diagnostic.ToString())));

        return compilation.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(new FileSystemApiAnalyzer())).GetAnalyzerDiagnosticsAsync();
    }
}
