using System;
using System.Collections.Generic;

namespace Domain.Helpers
{
    public class ConnectorsCombinationModel
    {
        public IEnumerable<ConnectorModel> Connectors { get; set; }
        public int NumberOfConnectors { get; set; }
        public int MaxCurrentInAmps { get; set; }
    }

    public class ConnectorModel
    {
        public Guid ChargeStationId { get; set; }
        public int ConnectorId { get; set; }
        public int MaxCurrentInAmps { get; set; }
    }
}
