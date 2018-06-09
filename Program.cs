using System;
using System.Collections.Generic;
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
            IList<VehicleTypeDto> vehicleTypes = new TypesService().GetVehicleTypes();
            Console.WriteLine("### input data");
            vehicleTypes.ForEach(x => Console.WriteLine(x));
            Console.WriteLine();

            // Generate enumeration class
            Console.WriteLine("### generated class");
            new CodeGenerator(Assembly.GetEntryAssembly().GetName().Name).CreateClass("VehicleType", vehicleTypes);

            // todo: 
            // Add cs file to csproj
            // https://stackoverflow.com/questions/18544354/how-to-programmatically-include-a-file-in-my-project

            // todo: work with generated class: output
            ////VehicleType car = VehicleType.Car;
            ////Console.WriteLine(car);

            Console.ReadLine();
        }
    }
}
