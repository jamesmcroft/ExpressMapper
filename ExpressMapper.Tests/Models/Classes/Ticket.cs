namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Ticket
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Venue Venue { get; set; }
    }
}
