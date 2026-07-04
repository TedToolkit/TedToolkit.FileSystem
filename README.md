# TedToolkit.FileSystem

[![License: LGPL-3.0](https://img.shields.io/badge/License-LGPL--3.0-blue.svg)](COPYING.LESSER)

`TedToolkit.FileSystem` is a .NET repository for a small file system abstraction library built around strongly typed path values and direct forwarding to the .NET Base Class Library.

The repository currently produces one NuGet package:

| Package | Purpose |
| --- | --- |
| `TedToolkit.FileSystem` | Strongly typed wrappers for file names, file paths, and directory paths with BCL-shaped APIs. |

For package usage and examples, see [src/TedToolkit.FileSystem/README.md](src/TedToolkit.FileSystem/README.md).

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

- `src/TedToolkit.FileSystem` contains the library and the package README.
- `tests/TedToolkit.FileSystem.Tests` contains the test project.
- `Build` contains the repository pipeline entry point.
- `externals/TedToolkit` is a submodule that supplies shared build props and infrastructure.

## Development Prerequisites

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

## Developer Notes

- The library is intentionally a forwarding layer over `System.IO` and related BCL APIs.
- Public APIs are expected to stay close to the underlying .NET shape instead of adding custom file system workflows.
- Target frameworks are imported from `externals/TedToolkit/props/AlmostAllFrameworks.props`.
- The project currently targets `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`, `net472`, `net48`, `netstandard2.0`, and `netstandard2.1`.
- The package-facing README lives at `src/TedToolkit.FileSystem/README.md` and should stay aligned with the actual public API surface.

## Packaging

The package project is [src/TedToolkit.FileSystem/TedToolkit.FileSystem.csproj](C:/PartTime/Code/TedToolkit/TedToolkit.FileSystem/src/TedToolkit.FileSystem/TedToolkit.FileSystem.csproj).

When changing the public API, update:

- XML documentation on the affected public members
- the package README in `src/TedToolkit.FileSystem/README.md`
- tests that describe the intended BCL-facing behavior

## License

Licensed under [LGPL-3.0](COPYING.LESSER). See [COPYING](COPYING) and [COPYING.LESSER](COPYING.LESSER) for the full license text.
