namespace ExpressMapper.Tests.Models.Classes
{
    using System;
    using System.Collections.Generic;

    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
