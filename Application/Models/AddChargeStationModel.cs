using System.Collections.Generic;

namespace Application.Models
{
    public class AddChargeStationModel
    {
        public string Name { get; set; }

        public IEnumerable<AddConnectorModel> Connectors { get; set; }
    }
}
