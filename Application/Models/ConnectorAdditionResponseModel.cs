using System;
using System.Collections.Generic;

namespace Application.Models
{
    public class ChargeStationAdditionResponseModel
    {
        public Guid? ChargeStationId { get; set; }

        public IEnumerable<SuggestionsOnAdditionFailureModel> Suggestions { get; set; }
    }

    public class ConnectorAdditionResponseModel
    {
        public int? ConnectorId { get; set; }
        
        public IEnumerable<SuggestionsOnAdditionFailureModel> Suggestions { get; set; }
    }

    public class SuggestionsOnAdditionFailureModel
    {
        public IEnumerable<ConnectorModel> ConnectorsToRemoveFromTheGroup { get; set; }

        public int NumberOfConnectors { get; set; }

        public int SumOfCurrentInAmps { get; set; }
    }

    public class ConnectorModel
    {
        public Guid ChargeStationId { get; set; }

        public int ConnectorId { get; set; }

        public int MaxCurrentInAmps { get; set; }
    }
}
