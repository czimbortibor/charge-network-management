using System;

namespace Domain.Entities
{
    public class Connector
    {
        public int Id { get; set; }

        public int MaxCurrentInAmps { get; set; }

        public Guid ChargeStationId { get; set; }
        public ChargeStation ChargeStation { get; set; }
    }
}
