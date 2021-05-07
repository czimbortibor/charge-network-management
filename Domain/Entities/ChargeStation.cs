using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class ChargeStation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<Connector> Connectors { get; set; }

        public ChargeStation()
        {
            Connectors = new List<Connector>();
        }


        public void AddConnector(Connector connector)
        {
            if (Connectors?.Count() >= 5)
            {
                throw new SpecificationException("Connector cannot be added as the charge station has a capacity of 5 connectors");
            }

            Connectors.Add(connector);
        }
    }
}
