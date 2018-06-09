// Class that is expected to be generated
// todo: remove
namespace XmlToCode
{
    public class ExampleEnum : Enumeration
    {
        public static readonly ExampleEnum Car
            = new ExampleEnum(1, "Car");
        public static readonly ExampleEnum Bicyle
            = new ExampleEnum(2, "Bicyle");
        public static readonly ExampleEnum Train
            = new ExampleEnum(3, "Train");
        public static readonly ExampleEnum Unknwon
            = new ExampleEnum(99, "Unknwon");

        private ExampleEnum() { }
        private ExampleEnum(int value, string displayName) : base(value, displayName) { }
    }
}
