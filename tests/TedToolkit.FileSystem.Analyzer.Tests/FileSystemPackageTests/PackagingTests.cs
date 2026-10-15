using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace TedToolkit.FileSystem.Analyzer.Tests.FileSystemPackageTests;

internal sealed class PackagingTests
{
    /// <summary>
    /// Verifies that clean default and redirected builds package the current analyzer and enable it in a NuGet consumer.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Should_include_current_analyzer_when_build_outputs_are_redirected_or_default(bool redirectOutputs)
    {
        var repositoryRoot = GetRepositoryRoot();
        var testRoot = Path.Combine(Path.GetTempPath(), "TedToolkit.FileSystem.PackageTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(testRoot);

        try
        {
            var sourceDirectory = Path.Combine(testRoot, "source");
            CopySourceFixture(repositoryRoot, sourceDirectory);
            await File.WriteAllTextAsync(Path.Combine(testRoot, "Directory.Build.props"), "<Project />");
            await File.WriteAllTextAsync(Path.Combine(testRoot, "Directory.Packages.props"), "<Project />");
            var version = "1.0.0-package-test-" + Guid.NewGuid().ToString("N");
            var arguments = new List<string>
            {
                "build", Path.Combine(sourceDirectory, "src", "TedToolkit.FileSystem", "TedToolkit.FileSystem.csproj"),
                "-c", "Release", "--nologo", "-m:1", "--disable-build-servers", "-v", "minimal",
                "-p:NuGetAudit=false", "-p:PackageVersion=" + version,
            };
            if (redirectOutputs)
                arguments.AddRange(["--artifacts-path", Path.Combine(testRoot, "output")]);

            var build = await RunDotNetAsync(testRoot, arguments.ToArray());

            await Assert.That(build.Output).DoesNotContain("error ");
            await Assert.That(build.ExitCode).IsEqualTo(0);
            arguments[0] = "pack";
            arguments.AddRange(["--no-build", "--no-restore"]);
            var pack = await RunDotNetAsync(testRoot, arguments.ToArray());
            await Assert.That(pack.Output).DoesNotContain("error ");
            await Assert.That(pack.ExitCode).IsEqualTo(0);
            var packagePath = Directory.GetFiles(testRoot, $"TedToolkit.FileSystem.{version}.nupkg", SearchOption.AllDirectories).Single();
            await VerifyPackageAsync(testRoot, packagePath);
            await VerifyConsumerAsync(testRoot, packagePath, version);
        }
        finally
        {
            Directory.Delete(testRoot, recursive: true);
        }
    }

    private static async Task VerifyPackageAsync(string testRoot, string packagePath)
    {
        const string analyzerName = "TedToolkit.FileSystem.Analyzer.dll";
        using var package = ZipFile.OpenRead(packagePath);
        await Assert.That(package.Entries.Count(static item => item.FullName == "analyzers/dotnet/cs/" + analyzerName)).IsEqualTo(1);
        var entry = package.GetEntry("analyzers/dotnet/cs/" + analyzerName);
        await Assert.That(entry is not null).IsTrue();
        using var packagedAssembly = entry!.Open();
        var builtAnalyzer = Directory.GetFiles(testRoot, analyzerName, SearchOption.AllDirectories)
            .Single(path => Path.GetRelativePath(testRoot, path).Split(Path.DirectorySeparatorChar).Contains("bin"));
        using var builtAssembly = File.OpenRead(builtAnalyzer);
        await Assert.That(Convert.ToHexString(SHA256.HashData(packagedAssembly)))
            .IsEqualTo(Convert.ToHexString(SHA256.HashData(builtAssembly)));

        using var nuspecStream = package.Entries.Single(static item => item.FullName.EndsWith(".nuspec", StringComparison.Ordinal)).Open();
        var nuspec = XDocument.Load(nuspecStream);
        await Assert.That(nuspec.Descendants().Any(static element =>
            element.Name.LocalName == "dependency" && (string?)element.Attribute("id") == "TedToolkit.FileSystem.Analyzer")).IsFalse();
    }

    private static async Task VerifyConsumerAsync(string testRoot, string packagePath, string version)
    {
        var cacheResult = await RunDotNetAsync(testRoot, "nuget", "locals", "global-packages", "--list", "--force-english-output");
        await Assert.That(cacheResult.ExitCode).IsEqualTo(0);
        var globalPackages = cacheResult.Output.Trim()["global-packages: ".Length..].Trim();
        var packages = Path.Combine(testRoot, "packages");
        var consumer = Path.Combine(testRoot, "consumer");
        Directory.CreateDirectory(consumer);
        await File.WriteAllTextAsync(Path.Combine(testRoot, "NuGet.Config"), $$"""
            <configuration>
              <packageSources>
                <clear />
                <add key="package" value="{{SecurityElement.Escape(Path.GetDirectoryName(packagePath))}}" />
                <add key="cached-dependencies" value="{{SecurityElement.Escape(globalPackages)}}" />
              </packageSources>
            </configuration>
            """);
        var project = Path.Combine(consumer, "Consumer.csproj");
        await File.WriteAllTextAsync(project, $$"""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <NuGetAudit>false</NuGetAudit>
                <RestorePackagesPath>{{SecurityElement.Escape(packages)}}</RestorePackagesPath>
              </PropertyGroup>
              <ItemGroup>
                <PackageReference Include="TedToolkit.FileSystem" Version="{{version}}" />
              </ItemGroup>
            </Project>
            """);
        var source = Path.Combine(consumer, "Consumer.cs");
        await File.WriteAllTextAsync(source, """
            internal static class Consumer
            {
                internal static object[] UseBcl() => new object[]
                {
                    System.IO.Directory.Exists("x"),
                    System.IO.File.Exists("x"),
                    System.IO.Path.Combine("x", "y"),
                    new System.IO.FileInfo("x"),
                    new System.IO.DirectoryInfo("x"),
                };
            }
            """);
        var blocked = await RunDotNetAsync(consumer, "build", project, "-c", "Release", "--nologo", "-m:1", "--disable-build-servers", "-v", "minimal");
        await Assert.That(blocked.ExitCode).IsNotEqualTo(0);
        foreach (var typeName in new[] { "Directory", "File", "Path", "FileInfo", "DirectoryInfo" })
            await Assert.That(blocked.Output).Contains($"error TTFS003: Do not use System.IO.{typeName} directly");

        var restoredPackage = Path.Combine(packages, "tedtoolkit.filesystem", version, $"tedtoolkit.filesystem.{version}.nupkg");
        await Assert.That(Convert.ToHexString(SHA256.HashData(await File.ReadAllBytesAsync(restoredPackage))))
            .IsEqualTo(Convert.ToHexString(SHA256.HashData(await File.ReadAllBytesAsync(packagePath))));
        await File.WriteAllTextAsync(source, """
            internal static class Consumer
            {
                internal static bool UsePaths(TedToolkit.FileSystem.FilePath file, TedToolkit.FileSystem.DirectoryPath directory) =>
                    file.Exists && directory.Exists;
            }
            """);
        await File.WriteAllTextAsync(Path.Combine(consumer, "Generated.g.cs"), """
            // <auto-generated/>
            internal static class GeneratedConsumer
            {
                internal static bool UseBcl() => System.IO.File.Exists("x");
            }
            """);
        var allowed = await RunDotNetAsync(consumer, "build", project, "-c", "Release", "--no-restore", "--nologo", "-m:1", "--disable-build-servers", "-v", "minimal");
        await Assert.That(allowed.Output).DoesNotContain("error ");
        await Assert.That(allowed.ExitCode).IsEqualTo(0);
    }

    private static void CopySourceFixture(string repositoryRoot, string sourceDirectory)
    {
        foreach (var relativePath in new[]
        {
            "src/TedToolkit.FileSystem", "src/TedToolkit.FileSystem.Analyzer",
            "externals/TedToolkit/props", "externals/TedToolkit/assets", "externals/TedToolkit/TedToolkit.CodeAnalysis",
        })
        {
            CopyDirectory(Path.Combine(repositoryRoot, relativePath), Path.Combine(sourceDirectory, relativePath));
        }

        foreach (var relativePath in new[]
        {
            "Directory.Build.props", "Directory.Packages.props", ".editorconfig",
            "externals/TedToolkit/Directory.Build.props", "externals/TedToolkit/Directory.Packages.props", "externals/TedToolkit/.editorconfig",
        })
        {
            var file = Path.Combine(repositoryRoot, relativePath);
            if (File.Exists(file))
                File.Copy(file, Path.Combine(sourceDirectory, relativePath));
        }
    }

    private static void CopyDirectory(string source, string destination)
    {
        Directory.CreateDirectory(destination);
        foreach (var file in Directory.EnumerateFiles(source))
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));

        foreach (var directory in Directory.EnumerateDirectories(source))
        {
            var name = Path.GetFileName(directory);
            if (name is not ("bin" or "obj" or ".git"))
                CopyDirectory(directory, Path.Combine(destination, name));
        }
    }

    private static string GetRepositoryRoot([CallerFilePath] string sourceFile = "") =>
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(sourceFile)!, "..", "..", ".."));

    private static async Task<(int ExitCode, string Output)> RunDotNetAsync(string workingDirectory, params string[] arguments)
    {
        var startInfo = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        foreach (var argument in arguments)
            startInfo.ArgumentList.Add(argument);

        using var process = Process.Start(startInfo)!;
        var standardOutput = process.StandardOutput.ReadToEndAsync();
        var standardError = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return (process.ExitCode, await standardOutput + await standardError);
    }
}
