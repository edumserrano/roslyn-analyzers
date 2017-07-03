using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Analyzers.Extensions
{
    internal static class ClassDeclarationSyntaxExtensions
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

        public static IEnumerable<EventFieldDeclarationSyntax> GetEventFiels(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .Where(x => x.IsKind(SyntaxKind.EventFieldDeclaration))
                .OfType<EventFieldDeclarationSyntax>();
        }

        public static IEnumerable<IndexerDeclarationSyntax> GetIndexers(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .Where(x => x.IsKind(SyntaxKind.IndexerDeclaration))
                .OfType<IndexerDeclarationSyntax>();
        }

        public static bool HasAbstractOrVirtualMethods(this ClassDeclarationSyntax classDeclaration)
        {
            var methods = classDeclaration.GetMethods();
            foreach (var m in methods)
            {
                var modifiers = m.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }

        public static bool HasAbstractOrVirtualProperties(this ClassDeclarationSyntax classDeclaration)
        {
            var props = classDeclaration.GetProperties();
            foreach (var p in props)
            {
                var modifiers = p.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }

        public static bool HasAbstractOrVirtualEvents(this ClassDeclarationSyntax classDeclaration)
        {
            var events = classDeclaration.GetEvents();
            foreach (var e in events)
            {
                var modifiers = e.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }

        public static bool HasAbstractOrVirtualEventFields(this ClassDeclarationSyntax classDeclaration)
        {
            var events = classDeclaration.GetEventFiels();
            foreach (var e in events)
            {
                var modifiers = e.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }

        public static bool HasAbstractOrVirtualIndexers(this ClassDeclarationSyntax classDeclaration)
        {
            var indexers = classDeclaration.GetIndexers();
            foreach (var i in indexers)
            {
                var modifiers = i.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }
    }
}
