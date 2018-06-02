namespace ExpressMapper.Tests.Models.Classes
{
    using System;
    using System.Collections.Generic;

    public class ItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubItem> SubItems { get; set; }
    }
}
