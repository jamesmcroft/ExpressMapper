﻿namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class TripCatalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TripType TripType { get; set; }
    }
}
