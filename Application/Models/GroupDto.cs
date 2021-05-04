using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Models
{
    public class GroupDto : IMapFrom<Group>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CapacityInAmps { get; set; }

        public IEnumerable<ChargeStationDto> ChargeStations { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupDto>()
                   .ForMember(x => x.ChargeStations, opt => opt.MapFrom(source => source.ChargeStations));
        }
    }
}
