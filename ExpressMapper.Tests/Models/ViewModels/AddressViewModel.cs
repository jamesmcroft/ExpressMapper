namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public CountryViewModel Country { get; set; }
        public string CountryName { get; set; }

    }
}
