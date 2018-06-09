namespace RoslynXmlToCode
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    class Program
    {
        static void Main(string[] args)
        {
            // Get input data
            XDocument xml = XDocument.Load("MetaData.xml");
            List<VehicleTypeDto> vehicleTypes = GetVehicleTypes(xml);
            Console.WriteLine("### input data");
            vehicleTypes.ForEach(x => Console.WriteLine(x));
            Console.WriteLine();

            // Goal: Generate a class like VehicleType from MetaData.xml resp. VehicleTypeDto
            Console.WriteLine("### generated class");
            CreateClass(vehicleTypes);

            // https://stackoverflow.com/questions/18544354/how-to-programmatically-include-a-file-in-my-project
            // Add cs file to csproj

            ////VehicleType car = VehicleType.Car;
            ////Console.WriteLine(car);

            Console.ReadLine();
        }

        // Code to SyntaxFactory code: http://roslynquoter.azurewebsites.net/
        // https://carlos.mendible.com/2017/03/02/create-a-class-with-net-core-and-roslyn/
        // https://github.com/dotnet/roslyn/wiki/Getting-Started-C%23-Syntax-Transformation
        private static void CreateClass(IList<VehicleTypeDto> vehicleTypes)
        {
            string currentAssemblyNamespace = Assembly.GetEntryAssembly().GetName().Name;

            // Create a namespace
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(currentAssemblyNamespace)).NormalizeWhitespace();

            // Add System using statement
            //@namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            //  Create a class
            var classDeclaration = SyntaxFactory.ClassDeclaration("VehicleType");

            // Add the public modifier
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Inherit Enumeration
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("Enumeration")));

            // Create members
            var memberX = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("VehicleType"))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("Car"))
                        .WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName("VehicleType"))
                            .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(1))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("Car")))
                                }))))))))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword), SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
            classDeclaration = classDeclaration.AddMembers(memberX);


            var ctor1 = SyntaxFactory.ConstructorDeclaration("VehicleType")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block());
            classDeclaration = classDeclaration.AddMembers(ctor1);

            var ctor2 = SyntaxFactory.ConstructorDeclaration("VehicleType")
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

        private static List<VehicleTypeDto> GetVehicleTypes(XDocument xml)
        {
            List<VehicleTypeDto> vehicleTypes = new List<VehicleTypeDto>();

            foreach (XElement item in xml.Root
                .Elements("VehicleTypes")
                .Elements("VehicleType"))
            {
                vehicleTypes.Add(new VehicleTypeDto(int.Parse(item.Attribute("id").Value), item.Attribute("name").Value));
            }

            return vehicleTypes;
        }

        private class VehicleTypeDto
        {
            private readonly int _id;
            private readonly string _name;

            public VehicleTypeDto(int id, string name)
            {
                _id = id;
                _name = name;
            }

            public int Id
            {
                get { return _id; }
            }

            public string Name
            {
                get { return _name; }
            }

            public override string ToString()
            {
                return string.Format("{0}:{1}", _id, _name);
            }
        }
    }
}
