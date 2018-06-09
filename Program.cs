using System;
using System.Collections.Generic;
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
            new ProjectAppender().
                AddFile(className, code);

            Console.WriteLine("### output new class");
            // todo: Doesn't work if code is generated with errors or the file doesn't exist
            VehicleType.GetAll().ToList().ForEach(x => Console.WriteLine(x));

            Console.ReadLine();
        }
    }
}
