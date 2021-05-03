﻿using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ChargeStation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        public ICollection<Connector> Connectors { get; set; }
    }
}