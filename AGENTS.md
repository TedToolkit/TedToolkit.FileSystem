# Project Instructions

## Language Policy

This is an English-only project.

- All code comments must be written in English.
- All documentation must be written in English.
- All README files must be written in English.
- Do not add Chinese comments, Chinese documentation, or mixed-language project text unless the user explicitly asks for it.

## File System API Design

- Public file system APIs should map directly to a corresponding .NET BCL member whenever possible.
- XML documentation for public file system APIs should use `remarks` to identify the underlying Microsoft API being wrapped.
- Avoid adding public convenience aliases or chained wrappers that only delegate to another custom API, except when expressing an equivalent BCL-shaped member as a property instead of a method.
- When a public API only exposes parameterless get/set semantics and does not have same-concept overloads that require arguments, model it as a property instead of a method.
- This library should remain a forwarding layer, not an algorithm or business-logic layer.
- Do not add standalone compatibility helper files such as `Compatibility.cs`; inline target-framework differences at the wrapped member with `#if` instead.
- Wrappers around `Path`, `Directory`, `File`, and similar BCL APIs should keep a single direct call in the member body whenever possible.
- Do not add explicit pre-validation or custom throw checks for direct wrappers; let the underlying BCL call validate arguments and fail naturally.
- If a target framework does not expose a wrapped BCL API such as `Path.GetRelativePath`, do not emulate it with `Uri` or any other custom logic; omit that wrapper on unsupported TFMs with `#if`.
- Add `MethodImpl(MethodImplOptions.AggressiveInlining)` to direct forwarding members and accessors.

## Test Execution

- Use `dotnet run --project <test-project>` to execute tests for this repository.
- Do not use `dotnet test` for this repository unless the user explicitly asks for it.
