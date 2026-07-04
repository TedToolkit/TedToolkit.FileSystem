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

## Test Execution

- Use `dotnet run --project <test-project>` to execute tests for this repository.
- Do not use `dotnet test` for this repository unless the user explicitly asks for it.
