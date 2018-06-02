namespace ExpressMapper.Tests.Models.Classes
{
    using System;
    using System.Collections.Generic;

    public class Engine
    {
        public Guid Id { get; set; }
        public string Capacity { get; set; }
        public List<Cylinder> Cylinders { get; set; }
    }
}
