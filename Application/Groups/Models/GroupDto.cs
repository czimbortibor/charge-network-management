﻿using System;

namespace Application.Groups.Models
{
    public class GroupDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CapacityInAmps { get; set; }
    }
}