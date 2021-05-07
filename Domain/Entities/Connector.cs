using System;

namespace Domain.Entities
{
    public class Connector
    {
        // TODO should be between 1 and 5
        public int Id { get; set; }

        public int MaxCurrentInAmps { get; set; }

        public Guid ChargeStationId { get; set; }
        public ChargeStation ChargeStation { get; set; }
    }
}
