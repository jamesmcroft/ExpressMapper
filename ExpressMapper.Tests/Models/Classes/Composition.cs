namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Composition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Booking Booking { get; set; }
    }
}
