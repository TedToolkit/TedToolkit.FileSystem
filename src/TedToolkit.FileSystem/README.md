# TedToolkit.FileSystem

`TedToolkit.FileSystem` provides a small path value object for .NET applications that want normalized separators and explicit absolute-path detection without passing raw strings everywhere.

## Install

```shell
dotnet add package TedToolkit.FileSystem
```

## Usage

```csharp
using TedToolkit.FileSystem;

var path = new FileSystemPath(@"folder\file.txt");

Console.WriteLine(path.Value);
Console.WriteLine(path.IsAbsolute);
```

## Public API

- `FileSystemPath`
