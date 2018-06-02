namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Booking
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Composition Composition { get; set; }
    }
}
