using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace TedToolkit.FileSystem.Analyzer;

/// <summary>
/// Reports direct references to BCL file system types that have strongly typed path replacements.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class FileSystemApiAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Identifies a reference to a BCL file system type.
    /// </summary>
    public const string DiagnosticId = "TTFS003";

    private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(
        DiagnosticId,
        "Use strongly typed file system paths",
        "Do not use System.IO.{0} directly; {1}",
        "TedToolkit.FileSystem",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Use FilePath and DirectoryPath to preserve file and directory path semantics.",
        helpLinkUri: "https://github.com/TedToolkit/TedToolkit.FileSystem#file-system-analyzer");

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(startContext =>
        {
            var blockedTypes = ImmutableArray.Create(
                startContext.Compilation.GetTypeByMetadataName("System.IO.Directory"),
                startContext.Compilation.GetTypeByMetadataName("System.IO.File"),
                startContext.Compilation.GetTypeByMetadataName("System.IO.Path"),
                startContext.Compilation.GetTypeByMetadataName("System.IO.FileInfo"),
                startContext.Compilation.GetTypeByMetadataName("System.IO.DirectoryInfo"));

            startContext.RegisterSyntaxNodeAction(nodeContext => AnalyzeTypeName(nodeContext, blockedTypes), SyntaxKind.IdentifierName);
            startContext.RegisterOperationAction(
                operationContext => AnalyzeOperation(operationContext, blockedTypes),
                OperationKind.Invocation,
                OperationKind.MethodReference,
                OperationKind.ObjectCreation,
                OperationKind.PropertyReference,
                OperationKind.FieldReference);
        });
    }

    private static void AnalyzeTypeName(SyntaxNodeAnalysisContext context, ImmutableArray<INamedTypeSymbol?> blockedTypes)
    {
        if (context.Node.IsPartOfStructuredTrivia())
            return;

        var symbol = context.SemanticModel.GetSymbolInfo(context.Node, context.CancellationToken).Symbol;
        if (symbol is INamedTypeSymbol type && IsBlockedType(type, blockedTypes))
            context.ReportDiagnostic(CreateDiagnostic(context.Node, type));
    }

    private static void AnalyzeOperation(OperationAnalysisContext context, ImmutableArray<INamedTypeSymbol?> blockedTypes)
    {
        var (member, instance) = context.Operation switch
        {
            IInvocationOperation invocation => ((ISymbol?)invocation.TargetMethod, invocation.Instance),
            IMethodReferenceOperation method => (method.Method, method.Instance),
            IObjectCreationOperation creation => (creation.Constructor, (IOperation?)null),
            IPropertyReferenceOperation property => (property.Property, property.Instance),
            IFieldReferenceOperation field => (field.Field, field.Instance),
            _ => ((ISymbol?)null, (IOperation?)null),
        };

        var type = member?.ContainingType;
        if (!IsBlockedType(type, blockedTypes))
        {
            // FileInfo and DirectoryInfo inherit members such as Exists and FullName from FileSystemInfo.
            type = type?.ContainingNamespace.ToDisplayString() == "System.IO" ? instance?.Type as INamedTypeSymbol : null;
            if (!IsBlockedType(type, blockedTypes))
                return;
        }

        // Explicit type names already receive a diagnostic, including aliases and qualified names.
        if (context.Operation.Syntax is ObjectCreationExpressionSyntax)
            return;

        var expression = context.Operation.Syntax is InvocationExpressionSyntax invocationSyntax
            ? invocationSyntax.Expression
            : context.Operation.Syntax;
        if (expression is MemberAccessExpressionSyntax access &&
            context.Operation.SemanticModel!
                .GetSymbolInfo(access.Expression, context.CancellationToken).Symbol is INamedTypeSymbol receiver &&
            IsBlockedType(receiver, blockedTypes))
        {
            return;
        }

        context.ReportDiagnostic(CreateDiagnostic(context.Operation.Syntax, type!));
    }

    private static bool IsBlockedType(INamedTypeSymbol? type, ImmutableArray<INamedTypeSymbol?> blockedTypes)
    {
        if (type is null)
            return false;

        foreach (var blockedType in blockedTypes)
        {
            if (SymbolEqualityComparer.Default.Equals(type, blockedType))
                return true;
        }

        return false;
    }

    private static Diagnostic CreateDiagnostic(SyntaxNode syntax, INamedTypeSymbol type)
    {
        var replacement = type.Name switch
        {
            "Directory" or "DirectoryInfo" => "use TedToolkit.FileSystem.DirectoryPath",
            "File" or "FileInfo" => "use TedToolkit.FileSystem.FilePath",
            _ => "use TedToolkit.FileSystem.DirectoryPath for directories or TedToolkit.FileSystem.FilePath for files",
        };

        return Diagnostic.Create(_rule, syntax.GetLocation(), type.Name, replacement);
    }
}
