using System.Collections.Generic;
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
            List<VehicleTypeDto> vehicleTypes = new List<VehicleTypeDto>();

            foreach (XElement item in xml.Root
                .Elements("VehicleTypes")
                .Elements("VehicleType"))
            {
                vehicleTypes.Add(new VehicleTypeDto(int.Parse(item.Attribute("id").Value), item.Attribute("name").Value));
            }

            return vehicleTypes;
        }
    }
}
