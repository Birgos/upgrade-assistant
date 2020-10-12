﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetMigrator.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = "AM001 CodeFix Provider"), Shared]
    public class UsingSystemWebFixer : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>ImmutableArray.Create(UsingSystemWebAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var node = root.FindNode(context.Span);

            if (node == null)
            {
                return;
            }

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    CodeFixResources.UsingSystemWebTitle,
                    cancellationToken => RemoveNodeAsync(context.Document, node, cancellationToken),
                    nameof(CodeFixResources.UsingSystemWebTitle)),
                context.Diagnostics);
        }

        private async Task<Document> RemoveNodeAsync(Document document, SyntaxNode node, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
            editor.RemoveNode(node, SyntaxRemoveOptions.KeepNoTrivia);
            return editor.GetChangedDocument();
        }
    }
}
