---
name: generate-typed-project-paths
description: >-
  Generate compile-time file and directory paths with TedToolkit.FileSystem.ProjectPaths and TedToolkitFileSystemPath MSBuild items.
---

# Generate Typed Project Paths

Select paths the program actually uses. Determine whether selection belongs in the consumer `.csproj` or in `Directory.Build.props` shared by several projects.

## Workflow

- Confirm the project is in a Git work tree and `git` is available to the build. Start with `ProjectPaths.Git` for the repository or `ProjectPaths.Project` for the project file.
- Select required files and directories with evaluated `TedToolkitFileSystemPath` items and the correct `Kind`. Selecting a directory does not select its children.
- Use explicit globs when children are needed, then build the consumer and inspect the generated members. Verify the generated namespace and identifiers before replacing literals.

## Example

```xml
<ItemGroup>
  <TedToolkitFileSystemPath Include="$(MSBuildThisFileDirectory)*.slnx" Kind="File" />
</ItemGroup>
```

```csharp
var repository = ProjectPaths.Git;
var project = ProjectPaths.Project;
var appProject = ProjectPaths.src.App.App_csproj;
```

## Check the result

If a generated member is absent, inspect the evaluated items and exclude generated output folders from broad globs. Verify the generated member under the same Git work tree and MSBuild configuration as CI.
