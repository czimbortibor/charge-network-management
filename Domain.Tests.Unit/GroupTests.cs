using Domain.Entities;
using Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Tests.Unit
{
    public class GroupTests
    {
        [Fact]
        public void UpdateCapacityInAmps_WithSufficientCapacity_PerformsTheUpdate()
        {
            int expectedCapacityInAmps = 150;

            var sut = new Group(name: "test group", capacityInAmps: 100);
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
            sut.AddChargeStation(chargeStation);

            sut.UpdateCapacityInAmps(expectedCapacityInAmps);

            Assert.Equal(expectedCapacityInAmps, sut.CapacityInAmps);
        }

        [Fact]
        public void UpdateCapacityInAmps_WithInsufficientCapacity_RejectsUpdate()
        {
            var sut = new Group(name: "test group", capacityInAmps: 100);
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
            sut.AddChargeStation(chargeStation);

            Assert.Throws<SpecificationException>(() => sut.UpdateCapacityInAmps(30));
        }

        [Fact]
        public void AddChargeStation_WithoutAConnector_RejectsAddition()
        {
            var sut = new Group(name: "test group", capacityInAmps: 130);
            var chargeStation = new ChargeStation
            {
                Connectors = new List<Connector>()
            };

            Assert.Throws<SpecificationException>(() => sut.AddChargeStation(chargeStation));
        }

        [Fact]
        public void GetMinimalAmountOfConnectorsForMaxCurrentInAmps_WithZeroConnectorsToBeAdded_GivesNoSuggestion()
        {
            var sut = new Group(name: "test group", capacityInAmps: 130);
            
            var result = sut.GetMinimalAmountOfConnectorsForMaxCurrentInAmps(50);

            Assert.Empty(result);
        }

        // TODO refactor with DataMember maybe?
        [Fact]
        public void GetMinimalAmountOfConnectorsForMaxCurrentInAmps_WithScenario1_ReturnsCorrectResult()
        {
            int groupCapacity = 50;
            var sut = new Group(name: "test group", groupCapacity);
            var chargeStation = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    }
                }
            };
            sut.ChargeStations.Add(chargeStation);

            int capacityToBeAdded = 50;
            int numberOfOptimalOptions = 1;
            int numberOfConnectorsToBeRemoved = 1;

            var result = sut.GetMinimalAmountOfConnectorsForMaxCurrentInAmps(capacityToBeAdded);

            Assert.Equal(numberOfOptimalOptions, result.Count());
            Assert.Equal(numberOfConnectorsToBeRemoved, result.Single().NumberOfConnectors);
            Assert.Equal(capacityToBeAdded, result.Single().MaxCurrentInAmps);
        }

        [Fact]
        public void GetMinimalAmountOfConnectorsForMaxCurrentInAmps_WithScenario2_ReturnsCorrectResult()
        {
            int groupCapacity = 50;
            var sut = new Group(name: "test group", groupCapacity);
            var chargeStation = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 20
                    }
                }
            };
            sut.ChargeStations.Add(chargeStation);

            int capacityToBeAdded = 30;
            int numberOfOptimalOptions = 3;
            int numberOfConnectorsToBeRemoved = 2;

            var result = sut.GetMinimalAmountOfConnectorsForMaxCurrentInAmps(capacityToBeAdded);

            Assert.Equal(numberOfOptimalOptions, result.Count());
            Assert.True(result.All(x => x.NumberOfConnectors == numberOfConnectorsToBeRemoved));
        }

        [Fact]
        public void GetMinimalAmountOfConnectorsForMaxCurrentInAmps_WithScenario3_ReturnsCorrectResult()
        {
            int groupCapacity = 50;
            var sut = new Group(name: "test group", groupCapacity);
            var chargeStation = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 20
                    }
                }
            };
            sut.ChargeStations.Add(chargeStation);

            int capacityToBeAdded = 35;
            int numberOfOptimalOptions = 0;

            var result = sut.GetMinimalAmountOfConnectorsForMaxCurrentInAmps(capacityToBeAdded);

            Assert.Equal(numberOfOptimalOptions, result.Count());
        }

        [Fact]
        public void GetMinimalAmountOfConnectorsForMaxCurrentInAmps_WithScenario4_ReturnsCorrectResult()
        {
            int groupCapacity = 120;
            var sut = new Group(name: "test group", groupCapacity);
            var chargeStationA = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    },
                }
            };
            sut.ChargeStations.Add(chargeStationA);

            var chargeStationB = new ChargeStation
            {
                Connectors = new List<Connector>
                {
                    new Connector
                    {
                        MaxCurrentInAmps = 10
                    },

                    new Connector
                    {
                        MaxCurrentInAmps = 50
                    }
                }
            };
            sut.ChargeStations.Add(chargeStationB);

            int capacityToBeAdded = 100;
            int numberOfOptimalOptions = 1;
            int numberOfConnectorsToBeRemoved = 2;

            var result = sut.GetMinimalAmountOfConnectorsForMaxCurrentInAmps(capacityToBeAdded);

            Assert.Equal(numberOfOptimalOptions, result.Count());
            Assert.True(result.All(x => x.NumberOfConnectors == numberOfConnectorsToBeRemoved));
        }
    }
}
