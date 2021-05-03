using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Group
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CapacityInAmps { get; set; }


        public ICollection<ChargeStation> ChargeStations { get; set; }
    }
}
