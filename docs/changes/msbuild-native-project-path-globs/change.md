# Use native MSBuild glob expansion for project paths

<!-- change-format: 3 -->
<!-- workflow-profile: controlled -->
<!-- change-kind: behavior-change -->
<!-- change-status: candidate-ready -->
<!-- delivery-shape: single -->

- Priority: P2
<!-- approval-source: user message 2026-09-11 confirming native MSBuild expansion and requesting implementation -->
<!-- candidate-binding: commit:14e0d3324cfdca1451b91fcc204b92c0516c032d -->

<!-- section: goal-rationale -->
## Goal and rationale

Consumers can select project files with ordinary unescaped MSBuild `Include`, `Exclude`, `Update`, and glob syntax. MSBuild owns file matching; the source generator consumes only the resulting concrete paths. This removes the current need to escape wildcards and eliminates the generator's duplicate glob implementation.

<!-- section: scope -->
## Scope and non-goals

- In scope: file selections declared through `TedToolkitFileSystemPath`, native MSBuild glob expansion, `Name` metadata, concrete path normalization against the Git work tree, package documentation, and MSBuild integration proof.
- Non-goals: directory glob expansion, changing the generated `ProjectPaths` API shape, changing member-name normalization, or adding a second public file-selection item type.
- Compatibility: `TedToolkitFileSystemPath`, `Kind`, `Name`, exact file selection, and exact directory selection remain supported, but every relative `Include` now follows native MSBuild project-relative semantics. Escaped wildcard items and Git-root-relative item identities are removed in the next major package version.
- Migration: move repository-wide declarations to the root `Directory.Build.props`; replace `$([MSBuild]::Escape('\*.slnx'))` with `$(MSBuildThisFileDirectory)*.slnx`, replace escaped recursive patterns in the same way, and prefix exact repository-root paths with `$(MSBuildThisFileDirectory)`.
- Release and recovery: ship the new contract as package version `2.0.0`; consumers that cannot migrate can pin the latest `1.x` package until their project files are updated.

<!-- section: behavior-contract -->
## Behavior contract

<!-- behavior-change: OB-01 -->
| ID | Observable boundary | Current | Expected | Preserved |
| --- | --- | --- | --- | --- |
| OB-01 | Consumer MSBuild path selection | Relative items use a generator-specific Git-root base and consumers must escape wildcards for generator-side expansion | All relative items use native MSBuild project-relative semantics; ordinary unescaped file globs expand before generation; `Exclude` and `Update` retain native MSBuild behavior | Exact file and directory item types, `Kind`, `Name`, Git-root containment, generated API shape, member naming, and collision diagnostics |

<!-- acceptance-case: AC-01 -->
### AC-01 — Unescaped file globs generate project paths

```gherkin
Scenario: A consumer selects repository files with native MSBuild globs
  Given a Git-backed consumer declares unescaped *.slnx and **\*.csproj items
  When the consumer project is built
  Then generated ProjectPaths members for the matching files compile successfully
```

<!-- acceptance-case: AC-02 -->
### AC-02 — Native item operations control the selected set

```gherkin
Scenario: A consumer refines native MSBuild file items
  Given a globbed item set uses Exclude and a concrete item uses Update with Name metadata
  When the consumer project is built
  Then excluded files are absent and the updated file uses the configured generated member name
```

<!-- acceptance-case: AC-03 -->
### AC-03 — Exact file and directory selections retain their generated types

```gherkin
Scenario: A consumer selects an exact file or directory
  Given project-relative or absolute TedToolkitFileSystemPath items contain no wildcard
  When the consumer project is built
  Then their current FilePath or DirectoryPath members remain available
```

<!-- acceptance-case: AC-04 -->
### AC-04 — Paths outside the Git work tree remain unavailable

```gherkin
Scenario: A consumer selects a concrete path outside its Git work tree
  Given an MSBuild item expands to a file outside the discovered Git root
  When the consumer project is built
  Then no generated ProjectPaths member exposes that file
```

## Constraints and risks

- MSBuild is the sole owner of file wildcard expansion; production generator code must not enumerate the repository to implement `*`, `**`, or `?`.
- File paths must cross the MSBuild-to-Roslyn boundary without a caller-authored escape expression.
- The package remains a source generator and must not introduce a runtime dependency or a custom file-scanning task.
- All relative item paths are project-relative according to MSBuild; repository-root declarations should use `$(MSBuildThisFileDirectory)` from a root `Directory.Build.props`.
- Removing legacy escaped-pattern and Git-root-relative behavior is a public compatibility change confined to package version `2.0.0`. Reintroducing it, removing exact directory selection, or adding a new public item type requires renewed approval.

<!-- section: start-conditions -->
## Start conditions

<!-- change-prerequisite: none -->

None. Ready from the approved baseline.

<!-- section: delivery-brief -->
## Delivery brief

- Outcome and target delivery area: align the package targets, incremental generator, tests, and package README around native MSBuild file items.
- Other real start conditions or resource prerequisites: .NET SDK, Git, and the existing TUnit integration-test infrastructure.
- Likely touchpoints (non-binding): `TedToolkit.FileSystem.ProjectPaths.targets`, `ProjectPathsGenerator.cs`, generator and MSBuild integration tests, package README, and package project contents if a new build asset is required.
- Private implementation choices left open: the exact Roslyn item transport and internal selected-path representation, provided callers see standard MSBuild behavior and no custom glob expansion remains.

<!-- section: proof-plan -->
## Proof

<!-- primary-proof: AC-01 purpose=acceptance shape=integration -->
<!-- primary-proof: AC-02 purpose=acceptance shape=integration -->
<!-- primary-proof: AC-03 purpose=regression shape=integration -->
<!-- primary-proof: AC-04 purpose=regression shape=integration -->
| Contract | Role | Observable assertion | Command or bounded procedure |
| --- | --- | --- | --- |
| AC-01 | Primary | A temporary Git-backed consumer using unescaped root and recursive globs compiles references to every matched generated member | `dotnet run --project tests/TedToolkit.FileSystem.ProjectPaths.Tests/TedToolkit.FileSystem.ProjectPaths.Tests.csproj -c Release` |
| AC-02 | Primary | The same build proves excluded members are unavailable and an updated item compiles under its configured name | Same integration-test command |
| AC-03 | Primary | Exact file and directory selections continue to generate their existing member types | Same integration-test command |
| AC-04 | Primary | A selected concrete file outside the temporary Git work tree does not appear in generated source | Same integration-test command |
| Existing generator behavior | Conditional | Existing member-name normalization and `TTFS002` collision tests remain green | Same test-project command |
| Affected package | Conditional | The repository solution builds with the packaged build assets and generator | `dotnet build TedToolkit.FileSystem.slnx -c Release` |

<!-- section: completion-criteria -->
## Completion

All acceptance cases and the affected solution build pass on one candidate. The package README documents the `1.x` to `2.0.0` migration, native project-relative semantics, root `Directory.Build.props` placement, and the durable ownership rule that MSBuild expands item operations while the generator consumes only concrete selected paths. No generator-side file glob implementation remains. The temporary change record has no retention exception and is eligible for normal post-merge cleanup.

## Verification result

- Implementation baseline: `d8bee38d1aded2c61dc50117ac7443b597058fda`.
- Review-remediation baseline: `0a0a1eaf148c8ce38ee7042cab1c924b88257552`.
- AC-01 through AC-04 and conditional generator regressions: `dotnet run --project tests/TedToolkit.FileSystem.ProjectPaths.Tests/TedToolkit.FileSystem.ProjectPaths.Tests.csproj -c Release` discovered 11 tests; 11 passed, 0 failed, 0 skipped on Windows with .NET 10.0.12.
- AC-03 now constrains the generated file and directory members through explicit `FilePath` and `DirectoryPath` assignments in the integration consumer.
- Structural package verification: `dotnet build TedToolkit.FileSystem.slnx -c Release` completed with 0 warnings and 0 errors and produced `TedToolkit.FileSystem.ProjectPaths.2.0.0.nupkg`.
- The packaged README contains the native MSBuild ownership rule, evaluated-item wording, aligned usage examples, and the 1.x-to-2.0 migration.
- The blocker found while reviewing `e0e4baea44130460b453b88674612d82a726cfe8` is resolved by canonical directory-boundary comparison, with regression proof for a valid `..cache` directory and rejected distinct-drive and UNC paths.
- Scope deviation: None.
