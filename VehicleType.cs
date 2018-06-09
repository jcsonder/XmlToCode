namespace XmlToCode
{
    using System.CodeDom.Compiler;

    [GeneratedCodeAttribute("CodeGenerator", "0.0.0.1")]
    public class VehicleType : Enumeration<VehicleType>
    {
        public static readonly VehicleType Undefined = new VehicleType(0, "Undefined");
        public static readonly VehicleType Car = new VehicleType(1, "Car");
        public static readonly VehicleType Bicycle = new VehicleType(2, "Bicycle");
        public static readonly VehicleType Train = new VehicleType(3, "Train");
        public static readonly VehicleType Plane = new VehicleType(4, "Plane");
        public static readonly VehicleType Rocket = new VehicleType(5, "Rocket");
        public static readonly VehicleType Tank = new VehicleType(6, "Tank");
        public static readonly VehicleType Skates = new VehicleType(7, "Skates");
        public VehicleType()
        {
        }

        public VehicleType(int value, string displayName): base(value, displayName)
        {
        }
    }
}