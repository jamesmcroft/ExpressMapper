namespace ExpressMapper.Tests.Models.Classes
{
    using System;
    using System.Collections.Generic;

    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime AgreementDate { get; set; }
        public int Rank { get; set; }

        public List<Size> Sizes { get; set; }
    }
}
