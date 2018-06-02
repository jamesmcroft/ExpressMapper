namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    using ExpressMapper.Tests.Models.Enums;

    public class UnitOfWork
    {
        public Guid Id { get; set; }
        public States State { get; set; }
    }
}
