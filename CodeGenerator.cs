using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace XmlToCode
{
    // ideas/snippets taken from
    // https://carlos.mendible.com/2017/03/02/create-a-class-with-net-core-and-roslyn/
    // https://github.com/dotnet/roslyn/wiki/Getting-Started-C%23-Syntax-Transformation
    // helper: Create SyntaxFactory code: http://roslynquoter.azurewebsites.net/
    internal class CodeGenerator
    {
        private string _namespaceName;

        public CodeGenerator(string namespaceName)
        {
            _namespaceName = namespaceName;
        }

        public void CreateClass(string typeName, IList<VehicleTypeDto> vehicleTypes)
        {
            // Create a namespace
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespaceName)).NormalizeWhitespace();

            // Add System using statement
            //@namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            //  Create a class
            var classDeclaration = SyntaxFactory.ClassDeclaration(typeName);

            // Add the public modifier
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Inherit Enumeration
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("Enumeration")));

            // Create members
            foreach (var vehicleType in vehicleTypes)
            {
                var memberX = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(typeName))
                        .WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(vehicleType.Name))
                            .WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(typeName))
                                .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(vehicleType.Id))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(vehicleType.Name)))
                                    }))))))))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword), SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
                classDeclaration = classDeclaration.AddMembers(memberX);
            }

            var ctor1 = SyntaxFactory.ConstructorDeclaration(typeName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block());
            classDeclaration = classDeclaration.AddMembers(ctor1);

            var ctor2 = SyntaxFactory.ConstructorDeclaration(typeName)
                .AddParameterListParameters(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("value"))
                        .WithType(SyntaxFactory.ParseTypeName("int")),
                                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("displayName"))
                        .WithType(SyntaxFactory.ParseTypeName("string")))
                .WithInitializer(
                    SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                        .AddArgumentListArguments(
                            SyntaxFactory.Argument(SyntaxFactory.IdentifierName("value")),
                            SyntaxFactory.Argument(SyntaxFactory.IdentifierName("displayName"))))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block());
            classDeclaration = classDeclaration.AddMembers(ctor2);

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Normalize and get code as string.
            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();

            // Output new code to the console.
            Console.WriteLine(code);
        }
    }
}
