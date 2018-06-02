namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    using ExpressMapper.Tests.Models.Structs;

    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Feature[] Features { get; set; }
    }
}
