namespace ExpressMapper.Tests.Models.Classes
{
    using System;
    using System.Collections.Generic;

    public class Table
    {
        public Guid Id;
        public string Name;
        public List<Brand> Brands;
        public Country Manufacturer;
        public List<Size> Sizes;
    }
}
