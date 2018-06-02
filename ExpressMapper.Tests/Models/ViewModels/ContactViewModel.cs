namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class ContactViewModel
    {
        public Guid Id { get; set; }

        public bool IsPerson { get; set; }

        public bool IsOrganization { get; set; }

        protected bool ContactEquals(ContactViewModel other)
        {
            return this.Id == other.Id && this.IsPerson == other.IsPerson && this.IsOrganization == other.IsOrganization;
        }
    }
}
