namespace ExpressMapper.Tests.Models.Structs
{
    using System;

    public struct Feature
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rank { get; set; }
    }
}
