using System.Diagnostics;

using TedToolkit.FileSystem.ProjectPaths;

namespace TedToolkit.FileSystem.ProjectPaths.Tests;

internal sealed class ProjectPathsMsBuildIntegrationTests
{
    /// <summary>
    /// Verifies that native MSBuild item operations select concrete project paths without escaped wildcards.
    /// </summary>
    [Test]
    public async Task Should_generate_project_paths_from_native_msbuild_items()
    {
        var testRoot = Path.Combine(Path.GetTempPath(), "TedToolkit.FileSystem.ProjectPaths.Tests", Guid.NewGuid().ToString("N"));
        var repositoryDirectory = Path.Combine(testRoot, "repository");
        var consumerDirectory = Path.Combine(repositoryDirectory, "src", "Consumer");
        var applicationDirectory = Path.Combine(repositoryDirectory, "src", "App");
        var excludedDirectory = Path.Combine(repositoryDirectory, "excluded");
        var assetsDirectory = Path.Combine(repositoryDirectory, "assets");
        var twoDotDirectory = Path.Combine(repositoryDirectory, "..cache");
        Directory.CreateDirectory(consumerDirectory);
        Directory.CreateDirectory(applicationDirectory);
        Directory.CreateDirectory(excludedDirectory);
        Directory.CreateDirectory(assetsDirectory);
        Directory.CreateDirectory(twoDotDirectory);

        try
        {
            var gitResult = await RunProcess("git", repositoryDirectory, "init", "--quiet");
            await Assert.That(gitResult.ExitCode).IsEqualTo(0);

            var projectFile = Path.Combine(consumerDirectory, "Consumer.csproj");
            var sourceFile = Path.Combine(consumerDirectory, "Program.cs");
            var generatedDirectory = Path.Combine(consumerDirectory, "generated");
            var analyzerPath = typeof(ProjectPathsGenerator).Assembly.Location;
            var fileSystemPath = typeof(DirectoryPath).Assembly.Location;
            var targetsPath = Path.Combine(AppContext.BaseDirectory, "TedToolkit.FileSystem.ProjectPaths.targets");
            var outsideFile = Path.Combine(testRoot, "outside.txt");

            await File.WriteAllTextAsync(Path.Combine(repositoryDirectory, "Repository.slnx"), "<Solution />");
            await File.WriteAllTextAsync(Path.Combine(applicationDirectory, "App.csproj"), "<Project />");
            await File.WriteAllTextAsync(Path.Combine(excludedDirectory, "Excluded.csproj"), "<Project />");
            await File.WriteAllTextAsync(Path.Combine(consumerDirectory, "local.txt"), string.Empty);
            await File.WriteAllTextAsync(Path.Combine(twoDotDirectory, "inside.txt"), string.Empty);
            await File.WriteAllTextAsync(outsideFile, string.Empty);
            await File.WriteAllTextAsync(
                Path.Combine(repositoryDirectory, "Directory.Build.props"),
                """
                <Project>
                  <ItemGroup>
                    <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)*.slnx" Kind="File" />
                    <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)**\*.csproj"
                                              Exclude="$(MSBuildThisFileDirectory)excluded\**\*.csproj"
                                              Kind="File" />
                    <TedToolkitFileSystemPath Update="$(MSBuildThisFileDirectory)src\App\App.csproj"
                                              Name="ApplicationProject" />
                    <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)assets" Kind="Directory" />
                    <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)..cache\*.txt" Kind="File" />
                    <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)..\outside.txt" Kind="File" />
                  </ItemGroup>
                </Project>
                """);
            await File.WriteAllTextAsync(
                projectFile,
                $$"""
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <OutputType>Exe</OutputType>
                    <TargetFramework>net10.0</TargetFramework>
                    <RootNamespace>Consumer</RootNamespace>
                    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
                    <CompilerGeneratedFilesOutputPath>{{XmlEscape(generatedDirectory)}}</CompilerGeneratedFilesOutputPath>
                  </PropertyGroup>
                  <ItemGroup>
                    <Reference Include="TedToolkit.FileSystem" HintPath="{{XmlEscape(fileSystemPath)}}" />
                    <Analyzer Include="{{XmlEscape(analyzerPath)}}" />
                    <TedToolkitFileSystemPath Include="local.txt" Kind="File" />
                  </ItemGroup>
                  <Import Project="{{XmlEscape(targetsPath)}}" />
                </Project>
                """);
            await File.WriteAllTextAsync(
                sourceFile,
                """
                using Consumer;

                _ = ProjectPaths.Repository_slnx;
                _ = ProjectPaths.src.Consumer.Consumer_csproj;
                _ = ProjectPaths.src.App.ApplicationProject;
                TedToolkit.FileSystem.FilePath localFile = ProjectPaths.src.Consumer.local_txt;
                TedToolkit.FileSystem.DirectoryPath assetsDirectory = ProjectPaths.assets.Directory;
                TedToolkit.FileSystem.FilePath twoDotDirectoryFile = ProjectPaths.cache.inside_txt;
                """);

            var buildResult = await RunProcess(
                "dotnet",
                consumerDirectory,
                "build",
                projectFile,
                "--configuration",
                "Release",
                "--nologo",
                "--verbosity",
                "minimal");

            await Assert.That(buildResult.Output).DoesNotContain("TTFS001");
            await Assert.That(buildResult.ExitCode).IsEqualTo(0);

            var generatedFile = Directory.GetFiles(generatedDirectory, "TedToolkit.FileSystem.ProjectPaths.g.cs", SearchOption.AllDirectories).Single();
            var generatedSource = await File.ReadAllTextAsync(generatedFile);
            await Assert.That(generatedSource).DoesNotContain("Excluded_csproj");
            await Assert.That(generatedSource).DoesNotContain("outside_txt");
        }
        finally
        {
            Directory.Delete(testRoot, recursive: true);
        }
    }

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
