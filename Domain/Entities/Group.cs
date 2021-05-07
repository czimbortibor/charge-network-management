using Domain.Exceptions;
using Domain.Helpers;
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

        // should not be this open for external changes...
        public ICollection<ChargeStation> ChargeStations { get; set; }

        public Group(string name, int capacityInAmps)
        {
            Name = name;
            CapacityInAmps = capacityInAmps;
            ChargeStations = new List<ChargeStation>();
        }

        public void AddChargeStation(ChargeStation chargeStation)
        {
            if (chargeStation.Connectors?.Any() == false)
            {
                throw new SpecificationException("Charge station cannot be added without at least a connector");
            }

            if (IsGroupCapacityEnoughForAdditionalChargeStation(chargeStation) == false)
            {
                throw new SpecificationException("Charge station cannot be added. The additional current load would be too much for the group");
            }

            ChargeStations.Add(chargeStation);
        }

        public bool IsGroupCapacityEnoughForAdditionalChargeStation(ChargeStation chargeStation)
        {
            int extraMaxCurrentInAmps = chargeStation.Connectors.Sum(x => x.MaxCurrentInAmps);

            int actualMaxCurrentOfConnectors = GetActualMaxCurrentOfConnectors();

            int resultingCapacity = actualMaxCurrentOfConnectors + extraMaxCurrentInAmps;

            bool canExtraChargeStationBeAdded = CapacityInAmps >= resultingCapacity;
            return canExtraChargeStationBeAdded;
        }

        public IEnumerable<ConnectorsCombinationModel> GetMinimalAmountOfConnectorsForMaxCurrentInAmps(int maxCurrentInAmps)
        {
            if (maxCurrentInAmps > CapacityInAmps || ChargeStations?.Any() == false)
            {
                return Enumerable.Empty<ConnectorsCombinationModel>();
            }

            var flatData = 
                ChargeStations
                .SelectMany(x => x.Connectors, (chargeStation, connector) => new ConnectorModel 
                { 
                    ChargeStationId = chargeStation.Id,
                    ConnectorId = connector.Id,
                    MaxCurrentInAmps = connector.MaxCurrentInAmps
                });

            var subsets = SubsetSum.Solve(flatData, maxCurrentInAmps);

            var result = subsets.Select(x => new ConnectorsCombinationModel
            {
                Connectors = x,
                MaxCurrentInAmps = x.Sum(y => y.MaxCurrentInAmps),
                NumberOfConnectors = x.Count()
            });
            return result;
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
                    ChargeStations
                    .SelectMany(x => x.Connectors, (station, connector) => connector.MaxCurrentInAmps)
                    .Sum();

            return currentCapacity;
        }
    }
}
