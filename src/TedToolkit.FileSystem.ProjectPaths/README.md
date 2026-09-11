# TedToolkit.FileSystem.ProjectPaths

`TedToolkit.FileSystem.ProjectPaths` generates strongly typed `DirectoryPath` and `FilePath` members for the files and directories you explicitly select from the current Git work tree.

It provides compile-time path access without hard-coding paths throughout an application:

```csharp
var repository = ProjectPaths.Git;
var project = ProjectPaths.Project;
var appProject = ProjectPaths.src.App.App_csproj;
```

Only paths selected by evaluated `TedToolkitFileSystemPath` items are generated. Selecting a directory never implicitly generates its children.

## Requirements

- The project must be inside a Git work tree.
- The `git` executable must be available on the build machine's `PATH`.
- The generated code uses `TedToolkit.FileSystem.DirectoryPath` and `TedToolkit.FileSystem.FilePath`; this dependency is included by the package.

## Install

Add the package to the project that needs the generated paths:

```xml
<PackageReference Include="TedToolkit.FileSystem.ProjectPaths" Version="*" PrivateAssets="all" />
```

## Select paths

Add `TedToolkitFileSystemPath` items for the files and directories that should become available through `ProjectPaths`. These are standard MSBuild items: relative paths are resolved from the project directory, and MSBuild expands `Include`, `Exclude`, `Update`, `*`, `?`, and `**` before the source generator runs.

For repository-wide selections, place the items in the repository root `Directory.Build.props` and anchor them with `$(MSBuildThisFileDirectory)`:

```xml
<ItemGroup>
  <!-- Includes every solution file in the repository root. -->
  <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)*.slnx" Kind="File" />

  <!-- Includes every project file except projects below generated output directories. -->
  <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)**/*.csproj"
                            Exclude="$(MSBuildThisFileDirectory)**/bin/**/*;$(MSBuildThisFileDirectory)**/obj/**/*"
                            Kind="File" />

  <!-- Generates one directory without implicitly selecting its contents. -->
  <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)src/App/Assets" Kind="Directory" />
</ItemGroup>
```

`Kind` accepts `File`, `Directory`, or `Any`; it defaults to `Any`. Use `File` or `Directory` when the expected path type is known.

MSBuild expands file globs into concrete items before generation:

| Token | Matches |
| --- | --- |
| `*` | Any characters within one path segment. `src/App/*.json` does not match files in child directories. |
| `?` | One character within one path segment. |
| `**` | Zero or more directory segments. `src/App/**/*.json` matches both `src/App/appsettings.json` and `src/App/Settings/feature.json`. |

Directory items must name a concrete directory; directory glob expansion is not provided. A selected directory generates its `Directory` property, but never generates its files or child directories unless files are selected by another item.

The generator consumes only the concrete paths selected by MSBuild. It does not scan the repository or implement a second glob matcher.

`Name` is optional and applies only to files. Use it to override a generated file member name when a file name would be unclear or conflicts with another file member:

```xml
<TedToolkitFileSystemPath Include="src/App/appsettings.Development.json"
                          Kind="File"
                          Name="DevelopmentSettings" />
```

When a file came from a glob, use standard MSBuild `Update` to set its name:

```xml
<TedToolkitFileSystemPath Update="$(MSBuildThisFileDirectory)src/App/appsettings.Development.json"
                          Name="DevelopmentSettings" />
```

## Migrating from 1.x

Version 1.x treated item values as Git-work-tree-relative patterns and required wildcard escaping. Version 2.0 uses native MSBuild item semantics.

Move repository-wide declarations to the root `Directory.Build.props` and replace escaped patterns:

```xml
<!-- 1.x -->
<TedToolkitFileSystemPath Include="$([MSBuild]::Escape('\*.slnx'))" Kind="File" />

<!-- 2.0 -->
<TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)*.slnx" Kind="File" />
```

Apply the same prefix to recursive patterns and exact repository-root paths. Consumers that cannot migrate immediately can remain on the latest 1.x package.

## Generated API

Every project receives one generated `ProjectPaths` class in the project's root namespace:

```csharp
public static class ProjectPaths
{
    public static DirectoryPath Git { get; }

    public static FilePath Project { get; }
}
```

- `ProjectPaths.Git` is the Git work tree root.
- `ProjectPaths.Project` is the `.csproj` file that references this package.
- Each generated directory is a static class with a `Directory: DirectoryPath` property.
- Each generated file is a `FilePath` property.

For the preceding configuration, usage looks like this:

```csharp
var assetsDirectory = ProjectPaths.src.App.Assets.Directory;
var appProject = ProjectPaths.src.App.App_csproj;
```

The generated `ProjectPaths` class is placed in the project's `RootNamespace`. Import that namespace before using it from another namespace:

```csharp
using MyProject;

var appProject = ProjectPaths.src.App.App_csproj;
```

Generated XML documentation contains the corresponding full path, so the path is visible in IntelliSense.

## Member names

Directory names preserve their original casing. File names also preserve casing, while characters that are not valid in C# identifiers are replaced with one underscore. Consecutive invalid characters produce only one underscore.

| Path name | Generated member name |
| --- | --- |
| `appsettings.Development.json` | `appsettings_Development_json` |
| `my  file-name.json` | `my_file_name_json` |
| `2026-report.json` | `_2026_report_json` |
| `class.json` | `_class_json` |

If two selected files produce the same member name, the build reports `TTFS002`. Set `Name` on one of the file items to resolve it.

## Diagnostics

| Id | Meaning |
| --- | --- |
| `TTFS001` | The project is not inside a Git work tree, or Git could not provide its root. |
| `TTFS002` | Two selected paths produce the same generated member name. |
