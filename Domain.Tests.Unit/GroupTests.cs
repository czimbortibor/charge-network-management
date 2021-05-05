using Domain.Entities;
using Domain.Exceptions;
using System.Collections.Generic;
using Xunit;

namespace Domain.Tests.Unit
{
    public class GroupTests
    {
        [Fact]
        public void UpdateCapacityInAmps_WithSufficientCapacity_PerformsTheUpdate()
        {
            int expectedCapacityInAmps = 150;

            var group = new Group(name: "test group", capacityInAmps: 100);
            var chargeStation = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    }
                }
            };
            group.AddChargeStation(chargeStation);

            group.UpdateCapacityInAmps(expectedCapacityInAmps);

            Assert.Equal(expectedCapacityInAmps, group.CapacityInAmps);
        }

        [Fact]
        public void UpdateCapacityInAmps_WithInsufficientCapacity_RejectsUpdate()
        {
            var group = new Group(name: "test group", capacityInAmps: 100);
            var chargeStation = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    }
                }
            };
            group.AddChargeStation(chargeStation);

            Assert.Throws<SpecificationException>(() => group.UpdateCapacityInAmps(30));
        }

        [Theory]
        [InlineData(30, true)]
        [InlineData(35, false)]
        public void IsGroupCapacityEnoughForExtraCurrent_UnderVariousScenarios_ReturnsCorrectResults(int extraCurrentInAmps, bool isGroupCapacityEnough)
        {
            var group = new Group(name: "test group", capacityInAmps: 130);
            var chargeStation = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    }
                }
            };
            group.AddChargeStation(chargeStation);

            bool result = group.IsGroupCapacityEnoughForExtraCurrent(extraCurrentInAmps);

            Assert.Equal(isGroupCapacityEnough, result);
        }
    }
}
