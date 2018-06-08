namespace RoslynXmlToCode
{
    public class VehicleType : Enumeration
    {
        public static readonly VehicleType Car
            = new VehicleType(1, "Car");
        public static readonly VehicleType Bicyle
            = new VehicleType(2, "Bicyle");
        public static readonly VehicleType Train
            = new VehicleType(3, "Train");
        public static readonly VehicleType Unknwon
            = new VehicleType(99, "Unknwon");

        private VehicleType() { }
        private VehicleType(int value, string displayName) : base(value, displayName) { }
    }
}
