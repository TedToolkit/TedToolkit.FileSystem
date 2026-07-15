# TedToolkit.FileSystem.ProjectPaths

`TedToolkit.FileSystem.ProjectPaths` generates strongly typed `DirectoryPath` and `FilePath` members for the files and directories you explicitly select from the current Git work tree.

It provides compile-time path access without hard-coding paths throughout an application:

```csharp
var repository = ProjectPaths.Git;
var project = ProjectPaths.Project;
var settings = ProjectPaths.src.App.appsettings_Development_json;
```

Only paths declared in the project file are generated. Selecting a directory never implicitly generates its children.

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

Add one `TedToolkitFileSystemPath` item for every file, directory, or pattern that should become available through `ProjectPaths`. Paths are relative to the Git work tree root, not to the project file.

```xml
<ItemGroup>
  <!-- Generates ProjectPaths.src.App.Assets.Directory only. -->
  <TedToolkitFileSystemPath Include="src/App/Assets" Kind="Directory" />

  <!-- Generates only matching JSON files and their required parent access chain. -->
  <TedToolkitFileSystemPath Include="src/App/*.json" Kind="File" />

  <!-- Includes JSON files in App and every child directory. -->
  <TedToolkitFileSystemPath Include="src/App/**/*.json" Kind="File" />

  <!-- Generates one specific file. -->
  <TedToolkitFileSystemPath Include="src/App/appsettings.Development.json" Kind="File" />
</ItemGroup>
```

`Kind` accepts `File`, `Directory`, or `Any`; it defaults to `Any`. Use `File` or `Directory` when the expected path type is known.

`Include` supports these glob tokens:

| Token | Matches |
| --- | --- |
| `*` | Any characters within one path segment. `src/App/*.json` does not match files in child directories. |
| `?` | One character within one path segment. |
| `**` | Zero or more directory segments. `src/App/**/*.json` matches both `src/App/appsettings.json` and `src/App/Settings/feature.json`. |

Patterns select only entries whose `Kind` matches. A selected directory generates its `Directory` property, but never generates its files or child directories unless they are selected by another item.

`Name` is optional and applies only to files. Use it to override a generated file member name when a file name would be unclear or conflicts with another file member:

```xml
<TedToolkitFileSystemPath Include="src/App/appsettings.Development.json"
                          Kind="File"
                          Name="DevelopmentSettings" />
```

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
var settingsFile = ProjectPaths.src.App.appsettings_Development_json;
```

The generated `ProjectPaths` class is placed in the project's `RootNamespace`. Import that namespace before using it from another namespace:

```csharp
using MyProject;

var configuration = ProjectPaths.src.App.appsettings_Development_json;
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
