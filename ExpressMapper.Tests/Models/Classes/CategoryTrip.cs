namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class CategoryTrip
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TripCatalog Catalog { get; set; }
    }
}
