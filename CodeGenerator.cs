using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public string CreateClass(string typeName, IList<VehicleTypeDto> vehicleTypes)
        {
            SyntaxFactory.Comment("Generated code");
            
            // Create a namespace
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_namespaceName)).NormalizeWhitespace();

            // Add System using statement
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.CodeDom.Compiler")));

            //  Create a class
            var classDeclaration = SyntaxFactory.ClassDeclaration(typeName);

            // add GeneratedCodeAttribute attribute
            classDeclaration = classDeclaration.AddAttributeLists(SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                    SyntaxFactory.Attribute(
                        SyntaxFactory.IdentifierName("GeneratedCodeAttribute"), 
                        SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(GetType().Name))),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("0.0.0.1")))
                            }))))));

            // Add the public modifier
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Inherit Enumeration
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"Enumeration<{typeName}>")));

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

            return code;
        }
    }
}
