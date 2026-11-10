---
name: generate-typed-project-paths
description: >-
  Generate compile-time file and directory paths with TedToolkit.FileSystem.ProjectPaths and TedToolkitFileSystemPath MSBuild items.
---

# Generate Typed Project Paths

- Confirm the project is in a Git work tree and `git` is available to the build. Start with `ProjectPaths.Git` for the repository or `ProjectPaths.Project` for the project file.
- Select required files and directories with evaluated `TedToolkitFileSystemPath` items and the correct `Kind`. Selecting a directory does not select its children.
- Use explicit globs when children are needed, then build the consumer and inspect the generated members. Verify the generated namespace and identifiers before replacing literals.
