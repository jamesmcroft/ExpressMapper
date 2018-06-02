namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Cylinder
    {
        public Guid Id { get; set; }
        public decimal Capacity { get; set; }
        public Engine Engine { get; set; }
    }
}
