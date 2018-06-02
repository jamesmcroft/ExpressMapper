namespace ExpressMapper.Tests.Models.Classes
{
    using System;
    using System.Collections.ObjectModel;

    public class Unit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Collection<SubUnit> SubUnits { get; set; }
    }
}
