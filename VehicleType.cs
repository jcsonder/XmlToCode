namespace XmlToCode
{
    public class VehicleType : Enumeration
    {
        public static readonly VehicleType Car = new VehicleType(1, "Car");
        public static readonly VehicleType Bicycle = new VehicleType(2, "Bicycle");
        public static readonly VehicleType Train = new VehicleType(3, "Train");
        public static readonly VehicleType Unknown = new VehicleType(99, "Unknown");
        public VehicleType()
        {
        }

        public VehicleType(int value, string displayName): base(value, displayName)
        {
        }
    }
}