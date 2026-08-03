using System.Diagnostics;

using TedToolkit.FileSystem.ProjectPaths;

namespace TedToolkit.FileSystem.ProjectPaths.Tests;

internal sealed class ProjectPathsMsBuildIntegrationTests
{
    /// <summary>
    /// Verifies that a Git-backed consumer receives generator properties before compilation.
    /// </summary>
    [Test]
    public async Task Should_generate_project_paths_when_consumer_is_inside_git_work_tree()
    {
        var repositoryDirectory = Path.Combine(Path.GetTempPath(), "TedToolkit.FileSystem.ProjectPaths.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(repositoryDirectory);

        try
        {
            var gitResult = await RunProcess("git", repositoryDirectory, "init", "--quiet");
            await Assert.That(gitResult.ExitCode).IsEqualTo(0);

            var projectFile = Path.Combine(repositoryDirectory, "Consumer.csproj");
            var sourceFile = Path.Combine(repositoryDirectory, "Program.cs");
            var analyzerPath = typeof(ProjectPathsGenerator).Assembly.Location;
            var fileSystemPath = typeof(DirectoryPath).Assembly.Location;
            var targetsPath = Path.Combine(AppContext.BaseDirectory, "TedToolkit.FileSystem.ProjectPaths.targets");

            await File.WriteAllTextAsync(projectFile, $$"""
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <OutputType>Exe</OutputType>
                    <TargetFramework>net10.0</TargetFramework>
                    <RootNamespace>Consumer</RootNamespace>
                  </PropertyGroup>
                  <ItemGroup>
                    <Reference Include="TedToolkit.FileSystem" HintPath="{{XmlEscape(fileSystemPath)}}" />
                    <Analyzer Include="{{XmlEscape(analyzerPath)}}" />
                  </ItemGroup>
                  <Import Project="{{XmlEscape(targetsPath)}}" />
                </Project>
                """);
            await File.WriteAllTextAsync(sourceFile, "using Consumer;\n\n_ = ProjectPaths.Git;\n");

            var buildResult = await RunProcess(
                "dotnet",
                repositoryDirectory,
                "build",
                projectFile,
                "--configuration",
                "Release",
                "--nologo",
                "--verbosity",
                "minimal");

            await Assert.That(buildResult.Output).DoesNotContain("TTFS001");
            await Assert.That(buildResult.ExitCode).IsEqualTo(0);
        }
        finally
        {
            Directory.Delete(repositoryDirectory, recursive: true);
        }
    }

    private static async Task<ProcessResult> RunProcess(string fileName, string workingDirectory, params string[] arguments)
    {
        var startInfo = new ProcessStartInfo(fileName)
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        foreach (var argument in arguments)
            startInfo.ArgumentList.Add(argument);

        using var process = Process.Start(startInfo)!;
        var standardOutput = process.StandardOutput.ReadToEndAsync();
        var standardError = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return new ProcessResult(process.ExitCode, await standardOutput + await standardError);
    }

    private static string XmlEscape(string value) => value
        .Replace("&", "&amp;")
        .Replace("\"", "&quot;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");

    private sealed record ProcessResult(int ExitCode, string Output);
}
