namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Trip
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CategoryTrip Category { get; set; }
    }
}
