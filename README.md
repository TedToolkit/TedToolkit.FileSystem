# TedToolkit.FileSystem

[![License: LGPL-3.0](https://img.shields.io/badge/License-LGPL--3.0-blue.svg)](COPYING.LESSER)

`TedToolkit.FileSystem` is a .NET repository for a small file system library built around strongly typed path values and direct forwarding to the .NET Base Class Library.

The repository currently ships one package:

| Package | Purpose |
| --- | --- |
| `TedToolkit.FileSystem` | Strongly typed wrappers for file names, file paths, and directory paths with BCL-shaped APIs. |

For package installation, examples, and public API usage, see [src/TedToolkit.FileSystem/README.md](src/TedToolkit.FileSystem/README.md).

## What This Repository Contains

This repository focuses on a thin file system abstraction layer for .NET applications that want clearer path semantics than raw strings without moving away from `System.IO`.

The library centers on three value types:

- `FileName`
- `FilePath`
- `DirectoryPath`

The public API is intentionally close to the BCL:

- wrap familiar `Path`, `File`, `Directory`, `FileInfo`, and `DirectoryInfo` members
- preserve direct forwarding behavior instead of adding custom file system workflows
- expose target-framework-specific APIs only where the underlying BCL member exists

## Repository Layout

```text
src/
  TedToolkit.FileSystem/
tests/
  TedToolkit.FileSystem.Tests/
Build/
  Build.csproj
externals/
  TedToolkit/
```

- `src/TedToolkit.FileSystem` contains the library and the NuGet package README.
- `tests/TedToolkit.FileSystem.Tests` contains the test project.
- `Build` contains the repository pipeline entry point.
- `externals/TedToolkit` is a submodule that provides shared props and build infrastructure.

## Prerequisites

- .NET SDK capable of building the repository target frameworks
- Git with submodule support

Clone with submodules:

```shell
git clone --recursive <repository-url>
cd TedToolkit.FileSystem
```

If the repository is already cloned without submodules:

```shell
git submodule update --init --recursive
```

## Build And Test

Build the solution:

```shell
dotnet build TedToolkit.FileSystem.slnx -c Release
```

Run the test project:

```shell
dotnet run --project tests/TedToolkit.FileSystem.Tests/TedToolkit.FileSystem.Tests.csproj -c Release
```

Run the repository pipeline:

```shell
dotnet run --project Build/Build.csproj
```

## Development Notes

- The library is a forwarding layer, not a higher-level file system framework.
- Public APIs should stay close to the underlying BCL member shape whenever possible.
- Wrapper behavior should come from the BCL call itself rather than custom pre-validation or emulation.
- The package-facing README in `src/TedToolkit.FileSystem/README.md` should stay aligned with the actual public API surface.

## Target Frameworks

The project currently targets:

- `net6.0`
- `net7.0`
- `net8.0`
- `net9.0`
- `net10.0`
- `net472`
- `net48`
- `netstandard2.0`
- `netstandard2.1`

## Release Checklist

Before publishing a package update, verify:

- public API changes are covered by tests
- XML documentation is updated for affected public members
- `src/TedToolkit.FileSystem/README.md` reflects the current public API and examples
- package metadata in [src/TedToolkit.FileSystem/TedToolkit.FileSystem.csproj](C:/PartTime/Code/TedToolkit/TedToolkit.FileSystem/src/TedToolkit.FileSystem/TedToolkit.FileSystem.csproj) still matches the intended release

## License

Licensed under [LGPL-3.0](COPYING.LESSER). See [COPYING](COPYING) and [COPYING.LESSER](COPYING.LESSER) for the full license text.
