namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Gift
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Person Recipient { get; set; }
    }
}
