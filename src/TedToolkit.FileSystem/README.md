# TedToolkit.FileSystem

`TedToolkit.FileSystem` provides small, strongly typed file system value objects for .NET while keeping the API close to `System.IO`.

Instead of passing raw strings everywhere, you can work with explicit path-focused values:

- `FileName`
- `FilePath`
- `DirectoryPath`

The library is designed as a thin forwarding layer. Most members map directly to a corresponding BCL API.

## Install

```shell
dotnet add package TedToolkit.FileSystem
```

## Why Use It

- Model file system values explicitly instead of passing ambiguous `string` parameters.
- Keep familiar `Path`, `File`, and `Directory` shaped operations close to the value you are already working with.
- Stay lighter than `FileInfo` and `DirectoryInfo` when you mainly want to pass around path values.
- Adopt stronger file system semantics without introducing a custom abstraction model on top of the BCL.

## Quick Start

```csharp
using TedToolkit.FileSystem;

var root = DirectoryPath.CurrentDirectory;
var logs = root / "logs";
var logFile = logs / "app".AsFileName("log");

logs.Create();
logFile.WriteAllText("Started");

Console.WriteLine(logFile.FullName);
Console.WriteLine(logFile.Extension);
```

## Core Types

### `FileName`

Represents a file name value.

```csharp
using TedToolkit.FileSystem;

FileName report = "report".AsFileName("txt");

Console.WriteLine(report.Name);
```

The extension argument follows `Path.ChangeExtension` semantics, so both `"txt"` and `".txt"` are valid.

### `DirectoryPath`

Represents a directory path and exposes directory-oriented operations.

```csharp
using TedToolkit.FileSystem;

var root = DirectoryPath.CurrentDirectory;
var artifacts = root / "artifacts";

artifacts.Create();

foreach (var file in artifacts.EnumerateFiles("*.json"))
{
    Console.WriteLine(file.FullName);
}
```

### `FilePath`

Represents a file path and exposes file-oriented operations.

```csharp
using TedToolkit.FileSystem;

var root = DirectoryPath.CurrentDirectory;
var file = root / "notes".AsFileName("txt");

file.WriteAllText("Hello from TedToolkit.FileSystem");

Console.WriteLine(file.ReadAllText());
Console.WriteLine(file.Name);
Console.WriteLine(file.NameWithoutExtension);
Console.WriteLine(file.ParentDirectory);
```

## Common Usage

### Build paths fluently

```csharp
using TedToolkit.FileSystem;

var configDirectory = DirectoryPath.ApplicationData / "MyApp";
var configFile = configDirectory / "settings".AsFileName("json");
```

### Resolve absolute and relative paths

```csharp
using TedToolkit.FileSystem;

var root = DirectoryPath.CurrentDirectory;
var relativeFile = new FilePath(@"data\sample.txt");

var absoluteFile = relativeFile.GetFullPath(root);
var relativeAgain = absoluteFile.GetRelativePath(root);

Console.WriteLine(absoluteFile.FullName);
Console.WriteLine(relativeAgain.FullName);
```

### Use BCL-shaped file operations from `FilePath`

```csharp
using TedToolkit.FileSystem;

var source = new FilePath("report.txt");
var backup = source.ChangeExtension(".bak");

source.CopyTo(backup, overwrite: true);
```

### Convert to `FileInfo` when needed

```csharp
using TedToolkit.FileSystem;

var file = new FilePath(@"artifacts\report.json");

Console.WriteLine(file.ToFileInfo().Length);
```

## Design Principles

- Public APIs stay intentionally close to `Path`, `File`, `Directory`, `FileInfo`, and `DirectoryInfo`.
- The library favors direct forwarding over custom validation, compatibility emulation, or higher-level workflows.
- Some members are available only on target frameworks where the wrapped BCL member exists.
- The goal is stronger typing and fluent composition, not replacing the BCL with a new file system model.

## Target Frameworks

`TedToolkit.FileSystem` targets:

- `net6.0`
- `net7.0`
- `net8.0`
- `net9.0`
- `net10.0`
- `net472`
- `net48`
- `netstandard2.0`
- `netstandard2.1`

## License

Licensed under LGPL-3.0.
