using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ChargeStation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }


        public ICollection<Connector> Connectors { get; set; }
    }
}
