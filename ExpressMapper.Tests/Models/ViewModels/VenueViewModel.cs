namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class VenueViewModel
    {
        public VenueViewModel(string name)
        {
            this.Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
