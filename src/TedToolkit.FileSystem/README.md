# TedToolkit.FileSystem

`TedToolkit.FileSystem` provides small, strongly typed file system value objects for .NET while keeping the API close to `System.IO`.

Instead of passing raw strings everywhere, you can work with:

- `FileName`
- `FilePath`
- `DirectoryPath`

The library is designed as a thin forwarding layer. Most members map directly to a corresponding BCL API.

## Install

```shell
dotnet add package TedToolkit.FileSystem
```

## Why Use It

- Use explicit path value types instead of ambiguous `string` parameters.
- Keep the convenience of `Path`, `File`, and `Directory` style APIs, but attach them to the value you are already working with.
- Stay lighter than `FileInfo` and `DirectoryInfo` when you mainly want to model and pass around path values.
- Write file system code in a more fluent, value-oriented style without moving away from familiar BCL concepts.

## Core Types

### `FileName`

Represents a file name value.

```csharp
using TedToolkit.FileSystem;

FileName logFile = "app".AsFileName(".log");

Console.WriteLine(logFile.Name);
```

### `DirectoryPath`

Represents a directory path and exposes directory-oriented operations.

```csharp
using TedToolkit.FileSystem;

var root = DirectoryPath.CurrentDirectory;
var logs = root / "logs";

logs.Create();

foreach (var file in logs.EnumerateFiles("*.log"))
{
    Console.WriteLine(file.FullName);
}
```

### `FilePath`

Represents a file path and exposes file-oriented operations.

```csharp
using TedToolkit.FileSystem;

var root = DirectoryPath.CurrentDirectory;
var file = root / "notes.txt".AsFileName();

file.WriteAllText("Hello from TedToolkit.FileSystem");

Console.WriteLine(file.ReadAllText());
Console.WriteLine(file.NameWithoutExtension);
Console.WriteLine(file.Extension);
Console.WriteLine(file.ParentDirectory);
```

## Common Usage

### Build paths without raw string plumbing

```csharp
using TedToolkit.FileSystem;

var configDirectory = DirectoryPath.ApplicationData / "MyApp";
var configFile = configDirectory / "settings.json".AsFileName();
```

### Resolve absolute and relative paths

```csharp
using TedToolkit.FileSystem;

var root = DirectoryPath.CurrentDirectory;
var relativeFile = new FilePath(@"data\sample.txt");

var absoluteFile = relativeFile.GetFullPath(root);
var relativeAgain = absoluteFile.GetRelativePath(root);
```

### Use BCL-shaped file operations from `FilePath`

```csharp
using TedToolkit.FileSystem;

var source = new FilePath("report.txt");
var destination = source.ChangeExtension(".bak");

source.CopyTo(destination, overwrite: true);
```

### Prefer path-focused values over `FileInfo` and `DirectoryInfo`

```csharp
using TedToolkit.FileSystem;

var file = new FilePath(@"artifacts\report.json");

Console.WriteLine(file.Name);
Console.WriteLine(file.NameWithoutExtension);
Console.WriteLine(file.ToFileInfo().Length);
```

## Design Notes

- Public APIs are intentionally close to `Path`, `File`, `Directory`, `FileInfo`, and `DirectoryInfo`.
- The types support a more fluent programming style by letting path values carry their own related operations.
- The library favors direct forwarding over custom validation or behavior emulation.
- Some APIs are available only on target frameworks where the underlying BCL member exists.

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
