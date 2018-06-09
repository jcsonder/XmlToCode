namespace XmlToCode
{
    internal class VehicleTypeDto
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
