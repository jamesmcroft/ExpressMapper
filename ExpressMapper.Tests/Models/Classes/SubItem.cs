namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class SubItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Unit[] Units { get; set; }
    }
}
