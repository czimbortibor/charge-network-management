using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Groups.Models
{
    public class GroupDto : IMapFrom<Group>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CapacityInAmps { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupDto>();
        }
    }
}
