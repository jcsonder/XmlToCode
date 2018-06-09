using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Console.WriteLine("### input data");
            IList<VehicleTypeDto> vehicleTypes = new TypesService()
                .GetVehicleTypes();
            vehicleTypes.ForEach(x => Console.WriteLine(x));
            Console.WriteLine();

            // Generate enumeration class
            Console.WriteLine("### generate class");
            string className = "VehicleType";
            var code = new CodeGenerator(Assembly.GetEntryAssembly().GetName().Name)
                .CreateClass(className, vehicleTypes);
            Console.WriteLine(code);

            Console.WriteLine("### save file");
            // todo: Write file: \\bin\\debug\
            // todo: Ensure we work in correct folder!
            string pathLevelAdjustment = "..\\..\\";
            string workingDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), pathLevelAdjustment));
            Console.WriteLine($"workingDirectory={workingDirectory}");
            string generatedClassFileName = $"{className}.cs";
            File.WriteAllText(Path.Combine(workingDirectory, generatedClassFileName), code);

            Console.WriteLine("### add file to csproj");
            // todo: 
            // Add cs file to csproj
            // https://stackoverflow.com/questions/18544354/how-to-programmatically-include-a-file-in-my-project
            // https://stackoverflow.com/questions/707107/parsing-visual-studio-solution-files

            // todo: get csproj file name dynamically
            // todo: Error InternalErrorException: https://github.com/Microsoft/msbuild/issues/1889 --> Solution: Install-Package Microsoft.Build.Utilities.Core -Version 15.1.1012
            var p = new Microsoft.Build.Evaluation.Project(Path.Combine(workingDirectory, "XmlToCode.csproj"));
            if (p.Items.FirstOrDefault(i => i.EvaluatedInclude == generatedClassFileName) == null)
            {
                p.AddItem("Compile", generatedClassFileName);
                p.Save();
            }

            Console.WriteLine("### output new class");
            // todo: work with generated class: output
            VehicleType.GetAll<VehicleType>().ToList().ForEach(x => Console.WriteLine(x));


            Console.ReadLine();
        }
    }
}
