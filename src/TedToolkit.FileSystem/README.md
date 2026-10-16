# TedToolkit.FileSystem

`TedToolkit.FileSystem` provides small, strongly typed file system value objects for .NET while keeping the API close to `System.IO`.

Instead of passing raw strings everywhere, you can work with explicit path-focused values:

- `FileName`
- `FilePath`
- `DirectoryPath`

The library is designed as a thin forwarding layer. Most members map directly to a corresponding BCL API.

## Native AOT

The .NET 8 and later runtime assets declare Native AOT compatibility and enable AOT, trimming, and single-file analysis.

`FilePath.FromAssembly`, `FilePath.EntryAssembly`, `FilePath.ExecutingAssembly`, and `FilePath.CallingAssembly` require a separately stored assembly file. These APIs remain available for ordinary applications, but carry `RequiresAssemblyFiles` annotations on .NET 6 and later so single-file and Native AOT consumers receive a diagnostic when calling them. For an application directory or executable path, use `AppContext.BaseDirectory` or `Environment.ProcessPath` according to the intended semantics.

## Install

```shell
dotnet add package TedToolkit.FileSystem
```

The package automatically enables a C# analyzer. Error `TTFS003` reports direct references to the following BCL types:

| BCL types | Replacement |
| --- | --- |
| `System.IO.Directory`, `System.IO.DirectoryInfo` | `DirectoryPath` |
| `System.IO.File`, `System.IO.FileInfo` | `FilePath` |
| `System.IO.Path` | `DirectoryPath` for directories or `FilePath` for files |

The rule covers type declarations, aliases, constructors, method groups, and member access, including inherited members accessed through `FileInfo` or `DirectoryInfo`. Generated code is excluded. Other `System.IO` types and unrelated types with the same names are allowed. At an interop boundary that requires a BCL type, suppress `TTFS003` locally with `#pragma warning disable TTFS003` and restore it after the affected code.

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


## Explicit precondition checks

Guard extensions are scoped to `FilePath`, `DirectoryPath`, their nullable forms, and their concrete result collections.
They capture the caller expression automatically and return `void` after successful validation.

```csharp
using TedToolkit.FileSystem;
using TedToolkit.Localizations;

LocalizationSettings.Culture = "zh-CN";
var inputDirectory = new DirectoryPath("inputs");
inputDirectory.ThrowIfNotExists();
inputDirectory.ThrowIfContainsNoFiles("*.json");
inputDirectory.ThrowIfContainsNoFiles("*.json", SearchOption.AllDirectories);

var files = inputDirectory.GetFiles("*.json");
files.ThrowIfNullOrHasNoItems();
```

| Extension | Failure condition |
| --- | --- |
| `ThrowIfNull()` | A nullable path has no value, or its stored path text is null, including default value objects |
| `ThrowIfNullOrWhiteSpace()` | The path is null, or its stored text is null, empty, or whitespace |
| `ThrowIfNotExists()` | Invalid path text, or no existing entry of the required kind |
| `ThrowIfExists()` | Invalid path text, or an existing file or directory occupies the path |
| `ThrowIfContainsNoFiles(searchPattern, searchOption)` | No matching files in the requested scope |
| `ThrowIfContainsNoFolders(searchPattern, searchOption)` | No matching subdirectories in the requested scope |
| `ThrowIfContainsNoEntries()` | Neither files nor child directories in the current directory |
| `ThrowIfNullOrHasNoItems()` | A filesystem result collection is null or its Count is zero |

Patterns use the existing native BCL wildcard rules from `Directory.EnumerateFiles` and
`Directory.EnumerateDirectories`, with `"*"` and `SearchOption.TopDirectoryOnly` as defaults.
Use `SearchOption.AllDirectories` for recursion. Enumeration stops at the first matching entry and
does not allocate a result array. A zero-byte file counts as a file; an empty child directory counts as an entry.

Path validation precedes directory existence, search-pattern validation, and content inspection.
Diagnostic names can be overridden with `paramName`; search-pattern names are captured separately.
Missing-entry diagnostics include the resolved absolute path, without changing the stored path value.
The collection overloads accept only `IReadOnlyCollection<FilePath>` and `IReadOnlyCollection<DirectoryPath>`
and read Count without enumerating or checking individual paths. They do not extend arbitrary objects,
strings, generic collections, or lazy sequences.

Translations use `TedToolkit.Localizations` end to end: JSON files registered as `AdditionalFiles`
generate the strongly typed `Localization.FileSystemGuards` API. Named placeholders are interpolated
by that generated API, without an independent resource manager or satellite resource assemblies.
Messages follow `LocalizationSettings.Culture`, with Chinese translations for `zh`, `zh-CN`, and
`zh-SG`; other cultures use the default English translation. In the default-language branch,
`LocalizationSettings.LocalizedStrings` can override keys such as `FileSystemGuards.FileNotFound`.
Guard operations reuse the existing typed `Exists`, `GetFullPath`, and enumeration members.
Only the compiler attributes needed for caller-expression capture and nullable flow are imported on
older targets; the Polyfill package adds no general extension methods or runtime dependency.
Existence checks follow `File.Exists` and `Directory.Exists` semantics and are not permission checks.
Checks observe the current filesystem state and do not reserve a path for later I/O.

The memory-only null, whitespace, and collection guards use `AggressiveInlining`, as do their
short validation helpers. Argument-failure message interpolation and exception allocation live in
private `NoInlining` methods annotated with `DoesNotReturn`. Filesystem I/O guards use the runtime's
normal inlining policy. These hints preserve the checks; their performance depends on the runtime
and call site.
