using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Models
{
    public class ChargeStationDto : IMapFrom<ChargeStation>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ConnectorDto> Connectors { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeStation, ChargeStationDto>()
                   .ForMember(x => x.Connectors, opt => opt.MapFrom(source => source.Connectors));
        }
    }
}
