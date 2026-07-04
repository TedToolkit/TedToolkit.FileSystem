# TedToolkit.FileSystem

[![License: LGPL-3.0](https://img.shields.io/badge/License-LGPL--3.0-blue.svg)](COPYING.LESSER)

`TedToolkit.FileSystem` contains a small .NET library for file-system-oriented domain types and helpers that are safer to pass around than raw path strings.

This repository currently ships one NuGet package:

| Package | Description |
| --- | --- |
| `TedToolkit.FileSystem` | Models normalized file system paths with explicit absolute path detection and stable separator normalization. |

## Package

### TedToolkit.FileSystem

```shell
dotnet add package TedToolkit.FileSystem
```

```csharp
using TedToolkit.FileSystem;

var path = new FileSystemPath(@"temp\demo.txt");

Console.WriteLine(path.Value);
Console.WriteLine(path.IsAbsolute);
```

Key public types:

- `FileSystemPath`

Target frameworks: imported from `externals/TedToolkit/props/AlmostAllFrameworks.props`.

Package documentation: [src/TedToolkit.FileSystem/README.md](src/TedToolkit.FileSystem/README.md)

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

The `externals/TedToolkit` submodule provides shared build props and repository build infrastructure.

## Development

Clone with submodules:

```shell
git clone --recursive <repository-url>
cd TedToolkit.FileSystem
```

If needed later:

```shell
git submodule update --init --recursive
```

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

## License

Licensed under [LGPL-3.0](COPYING.LESSER). See [COPYING](COPYING) and [COPYING.LESSER](COPYING.LESSER).
