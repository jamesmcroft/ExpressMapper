namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class TicketViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public VenueViewModel Venue { get; set; }
    }
}
