using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.Models
{
    public class ConnectorDto : IMapFrom<Connector>
    {
        public int Id { get; set; }

        public int MaxCurrentInAmps { get; set; }
    }
}
