﻿namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Address
    {
        public Guid Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }

    }
}
