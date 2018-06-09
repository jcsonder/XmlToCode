using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using XmlToCode;

namespace RoslynXmlToCode
{
    class Program
    {
        // Generate an enum class from a MetaData.xml
        static void Main(string[] args)
        {
            // Get input data
            IList<VehicleTypeDto> vehicleTypes = new TypesService()
                .GetVehicleTypes();
            Console.WriteLine("### input data");
            vehicleTypes.ForEach(x => Console.WriteLine(x));
            Console.WriteLine();

            // Generate enumeration class
            string className = "VehicleType";
            Console.WriteLine("### generated class");
            var code = new CodeGenerator(Assembly.GetEntryAssembly().GetName().Name)
                .CreateClass(className, vehicleTypes);
            Console.WriteLine(code);

            // todo: Write file: \\bin\\debug\
            // todo: Ensure we work in correct folder!
            string pathLevelAdjustment = "..\\..\\";
            string workingDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), pathLevelAdjustment));
            Console.WriteLine($"workingDirectory={workingDirectory}");
            File.WriteAllText(Path.Combine(workingDirectory, $"{className}.cs"), code);

            // todo: 
            // Add cs file to csproj
            // https://stackoverflow.com/questions/18544354/how-to-programmatically-include-a-file-in-my-project
            // https://stackoverflow.com/questions/707107/parsing-visual-studio-solution-files

            // todo: get csproj file name dynamically
            // todo: Error InternalErrorException: https://github.com/Microsoft/msbuild/issues/1889 --> .NET Core
            var p = new Microsoft.Build.Evaluation.Project(Path.Combine(workingDirectory, "XmlToCode.csproj"));
            // todo: Add only if not already existing: Folder & file
            p.AddItem("Folder", @"C:\projects\BabDb\test\test2");
            ////p.AddItem("Compile", @"C:\projects\BabDb\test\test2\Class1.cs");
            ////p.Save();

            // todo: work with generated class: output
            ////VehicleType car = VehicleType.Car;
            ////Console.WriteLine(car);

            Console.ReadLine();
        }
    }
}
