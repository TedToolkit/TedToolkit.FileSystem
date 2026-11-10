---
name: use-typed-file-system-paths
description: >-
  Use TedToolkit.FileSystem path values for C# file and directory operations, including projects reporting TTFS003 for direct System.IO calls.
---

# Use Typed File System Paths

Use the narrow path type at the boundary where the input becomes a file name, file path, or directory path. The wrappers preserve BCL forwarding semantics.

## Workflow

- Choose `FileName`, `FilePath`, or `DirectoryPath` to represent the required value. Keep paths typed through application calls and convert at boundaries that require strings.
- Use the matching BCL-shaped member on the path value. Build all affected target frameworks when using an API that may not exist on older frameworks.
- For Native AOT or single-file applications, choose the executable path or application base directory according to the actual intent before using assembly-location APIs.
- Verify the success and failure cases at the same path boundary so the wrapper preserves the expected BCL behavior.

## Example

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

## Check the result

When replacing a direct `System.IO` call, confirm that the path wrapper exposes the same overload for each supported target. Verify missing paths, overwrite behavior, and the exception expected by the caller.
