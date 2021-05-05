using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Group
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CapacityInAmps { get; private set; }

        private List<ChargeStation> _chargeStations;
        public IReadOnlyCollection<ChargeStation> ChargeStations => _chargeStations;

        public Group(string name, int capacityInAmps)
        {
            Name = name;
            CapacityInAmps = capacityInAmps;
            _chargeStations = new List<ChargeStation>();
        }

        public void AddChargeStation(ChargeStation chargeStation)
        {
            _chargeStations.Add(chargeStation);
        }

        public bool IsGroupCapacityEnoughForExtraCurrent(int extraMaxCurrentInAmps)
        {
            int actualMaxCurrentOfConnectors = GetActualMaxCurrentOfConnectors();

            int resultingCapacity = actualMaxCurrentOfConnectors + extraMaxCurrentInAmps;

            bool canExtraCurrentBeAdded = CapacityInAmps >= resultingCapacity;
            return canExtraCurrentBeAdded;
        }

        public void UpdateCapacityInAmps(int capacityInAmps)
        {
            int actualMaxCurrentOfConnectors = GetActualMaxCurrentOfConnectors();

            bool canCapacityBeUpdated = capacityInAmps >= actualMaxCurrentOfConnectors;
            if (canCapacityBeUpdated == false)
            {
                throw new SpecificationException($"Capacity cannot be decreased as the attached connectors require a capacity of at least: {actualMaxCurrentOfConnectors} amps");
            }

            CapacityInAmps = capacityInAmps;
        }

        private int GetActualMaxCurrentOfConnectors()
        {
            int currentCapacity = 
                    _chargeStations
                    .SelectMany(x => x.Connectors, (station, connector) => connector.MaxCurrentInAmps)
                    .Sum();

            return currentCapacity;
        }
    }
}
