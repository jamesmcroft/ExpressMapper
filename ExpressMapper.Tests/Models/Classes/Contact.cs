namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Contact
    {
        public Guid Id { get; set; }

        public bool IsPerson { get; set; }

        public bool IsOrganization { get; set; }
    }
}
