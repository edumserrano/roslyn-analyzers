using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Analyzers.Extensions
{
    public static class ClassDeclarationSyntaxExtensions
    {
        public static bool IsStatic(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Modifiers
                .Any(x => x.IsKind(SyntaxKind.StaticKeyword));
        }

        public static bool IsSealed(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Modifiers
                .Any(x => x.IsKind(SyntaxKind.SealedKeyword));
        }

        public static IEnumerable<MethodDeclarationSyntax> GetMethods(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .Where(x => x.IsKind(SyntaxKind.MethodDeclaration))
                .OfType<MethodDeclarationSyntax>();
        }

        public static IEnumerable<PropertyDeclarationSyntax> GetProperties(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .Where(x => x.IsKind(SyntaxKind.PropertyDeclaration))
                .OfType<PropertyDeclarationSyntax>();
        }

        public static IEnumerable<EventDeclarationSyntax> GetEvents(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .Where(x => x.IsKind(SyntaxKind.EventDeclaration))
                .OfType<EventDeclarationSyntax>();
        }

        public static IEnumerable<IndexerDeclarationSyntax> GetIndexers(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .Where(x => x.IsKind(SyntaxKind.IndexerDeclaration))
                .OfType<IndexerDeclarationSyntax>();
        }
    }
}
