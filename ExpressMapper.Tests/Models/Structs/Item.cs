namespace ExpressMapper.Tests.Models.Structs
{
    using System;
    using System.Collections.Generic;

    public struct Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public List<Feature> Features { get; set; }
    }
}
