namespace RoslynXmlToCode
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    class Program
    {
        static void Main(string[] args)
        {
            XDocument xml = XDocument.Load("MetaData.xml");
            List<VehicleTypeDto> vehicleTypes = GetVehicleTypes(xml);
            ConsoleWrite(vehicleTypes);

            // Goal: Generate a class like VehicleType from MetaData.xml resp. VehicleTypeDto

            VehicleType car = VehicleType.Car;
            Console.WriteLine(car);

            CreateClass();

            // https://stackoverflow.com/questions/18544354/how-to-programmatically-include-a-file-in-my-project
            // Add cs file to csproj

            Console.ReadLine();
        }

        // https://carlos.mendible.com/2017/03/02/create-a-class-with-net-core-and-roslyn/
        // https://github.com/dotnet/roslyn/wiki/Getting-Started-C%23-Syntax-Transformation
        private static void CreateClass()
        {
            // Create a namespace: (namespace CodeGenerationSample)
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("MyNamespace")).NormalizeWhitespace();

            // Add System using statement: (using System)
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            //  Create a class: (class Order)
            var classDeclaration = SyntaxFactory.ClassDeclaration("Order");

            // Add the public modifier: (public class Order)
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Inherit BaseEntity<T> and implement IHaveIdentity: (public class Order : BaseEntity<T>, IHaveIdentity)
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("BaseEntity<Order>")),
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("IHaveIdentity")));

            // Create a string variable: (bool canceled;)
            var variableDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName("bool"))
                .AddVariables(SyntaxFactory.VariableDeclarator("canceled"));

            // Create a field declaration: (private bool canceled;)
            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));

            // Create a Property: (public int Quantity { get; set; })
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("int"), "Quantity")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            // Create a stament with the body of a method.
            var syntax = SyntaxFactory.ParseStatement("canceled = true;");

            // Create a method
            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "MarkAsCanceled")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(syntax));

            // Add the field, the property and method to the class.
            classDeclaration = classDeclaration.AddMembers(fieldDeclaration, propertyDeclaration, methodDeclaration);

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Normalize and get code as string.
            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();

            // Output new code to the console.
            Console.WriteLine(code);
        }

        private static void ConsoleWrite(List<VehicleTypeDto> vehicleTypes)
        {
            foreach (VehicleTypeDto vehicleType in vehicleTypes)
            {
                Console.WriteLine(vehicleType);
            }
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
