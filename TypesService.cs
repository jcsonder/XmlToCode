using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XmlToCode
{
    internal class TypesService
    {
        public IList<VehicleTypeDto> GetVehicleTypes()
        {
            XDocument xml = XDocument.Load("MetaData.xml");
            return GetVehicleTypes(xml);
        }

        private IList<VehicleTypeDto> GetVehicleTypes(XDocument xml)
        {
            return xml.Root
                .Elements("VehicleTypes")
                .Elements("VehicleType")
                .Select(x => new VehicleTypeDto(int.Parse(x.Attribute("id").Value), x.Attribute("name").Value))
                .ToList();
        }
    }
}
